using AutoMapper;
using EventsApp.BLL.Exceptions;
using EventsApp.BLL.Interfaces.Auth;
using EventsApp.Configuration;
using EventsApp.DAL.Entities;
using EventsApp.DAL.Interfaces;
using EventsApp.Domain.Models.Auth;
using EventsApp.Domain.Models.Participants;
using EventsApp.Domain.Models.RefreshTokens;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventsApp.BLL.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<AuthService> _logger;
    private readonly AuthSettings _authSettings;
    private readonly IMapper _mapper;

    // TODO: вынести в конфигурацию
    private const int LifeTimeInMinutes = 30;

    public AuthService(
        IUserRepository userRepository, 
        IPasswordHashService passwordHashService,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<AuthSettings> authSettings, 
        ILogger<AuthService> logger, IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
        _mapper = mapper;
        _authSettings = authSettings.Value;
    }

    public async Task<AuthTokenModel> SignIn(SignInModel signInModel, CancellationToken cancellationToken)
    {
        var userSignInDetails = _mapper.Map<UserModel>(await _userRepository
            .GetByEmailAsync(signInModel.Email, cancellationToken));

        if (userSignInDetails is null)
        {
            throw new NotFoundException($"Пользователь с email {signInModel.Email} не найден");
        }
        if (!_passwordHashService.Verify(signInModel.Password, userSignInDetails.PasswordHash)) 
        {
            throw new InvalidOperationException("Неверный пароль");
        }
        if (await _refreshTokenRepository.GetByUserIdAsync(userSignInDetails.Id, cancellationToken) is not null) 
        {
            throw new InvalidOperationException($"У пользователя с email {userSignInDetails.Email} есть " +
                $"действующий refresh token");
        }
        
        // сгенерируем access token
        var authAccessTokenModel = AuthTokenGenerator.GenerateAccessToken(new GenerateTokenPayload
        {
            UserId = userSignInDetails.Id,
            UserRole = userSignInDetails.Role
        }, _authSettings);
        
        // сгенерируем refresh token
        var refreshToken = AuthTokenGenerator.GenerateRefreshToken();
        
        var expiryDate = DateTime.UtcNow.AddMinutes(LifeTimeInMinutes);
        var refreshTokenModel = new RefreshTokenModel
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            ExpiryDate = expiryDate,
            CreatedDate = DateTime.UtcNow,
            UserId = userSignInDetails.Id
        };
        
        _ = await _refreshTokenRepository
            .AddAsync(_mapper.Map<RefreshTokenEntity>(refreshTokenModel), cancellationToken);
        
        return new AuthTokenModel
        {
            AuthAccessTokenModel = authAccessTokenModel,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthTokenModel> GetNewTokensPairAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenModel = _mapper.Map<RefreshTokenModel>(await _refreshTokenRepository
            .GetByTokenAsync(refreshToken, cancellationToken));
        
        if (refreshTokenModel is null) 
        {
            throw new InvalidOperationException($"Сущность токена {refreshToken} не найдена");
        }

        var userDetails = _mapper.Map<UserModel>(await _userRepository
            .GetByIdAsync(refreshTokenModel.UserId, cancellationToken));
        
        if (userDetails is null) 
        {
            throw new InvalidOperationException($"Пользователь с Id {refreshTokenModel.UserId} не найден.");
        }
        
        // сгенерируем новую пару токенов
        var newAuthAccessTokenModel = AuthTokenGenerator.GenerateAccessToken(new GenerateTokenPayload
        {
            UserId = userDetails.Id,
            UserRole = userDetails.Role
        }, _authSettings);
        var newRefreshToken = AuthTokenGenerator.GenerateRefreshToken();
        
        var expiryDate = DateTime.UtcNow.AddMinutes(LifeTimeInMinutes);
        var newRefreshTokenModel = new RefreshTokenModel
        {
            Id = Guid.NewGuid(),
            Token = newRefreshToken,
            ExpiryDate = expiryDate,
            CreatedDate = DateTime.UtcNow,
            UserId = userDetails.Id
        };
        
        // удалим старый токен пользователя и добавим новый
        _ = await _refreshTokenRepository
            .DeleteAsync(_mapper.Map<RefreshTokenEntity>(refreshTokenModel), cancellationToken);
        _ = await _refreshTokenRepository
            .AddAsync(_mapper.Map<RefreshTokenEntity>(newRefreshTokenModel), cancellationToken);
        
        return new AuthTokenModel
        {
            AuthAccessTokenModel = newAuthAccessTokenModel,
            RefreshToken = newRefreshToken
        };
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenModel = _mapper.Map<RefreshTokenModel>(await _refreshTokenRepository
            .GetByTokenAsync(refreshToken, cancellationToken));
        
        if (refreshTokenModel is null)
        {
            _logger.LogInformation("Сущность токена {token} не найдена", refreshToken);
            throw new NotFoundException($"Сущность токена {refreshToken} не найдена");
        }
        
        _ = await _refreshTokenRepository
            .DeleteAsync(_mapper.Map<RefreshTokenEntity>(refreshTokenModel), cancellationToken);
        
        _logger.LogInformation("Сущность токена {token} успешно удалена", refreshTokenModel.Token);
    }

    public async Task SignUpAsync(UserModel userModel, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByEmailAsync(userModel.Email, cancellationToken) is not null)
        {
            throw new InvalidOperationException($"Пользователь с Email {userModel.Email} уже существует");
        }
        
        userModel.Id = Guid.NewGuid();
        userModel.PasswordHash = _passwordHashService.HashPassword(userModel.PasswordHash);
        userModel.BirthDate = userModel.BirthDate.ToUniversalTime();
        userModel.Role = UserRole.Default;
        
        await _userRepository.AddAsync(_mapper.Map<UserEntity>(userModel), cancellationToken);
    }
}
