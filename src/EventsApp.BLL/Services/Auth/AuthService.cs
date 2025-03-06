using EventsApp.BLL.Interfaces.Auth;
using EventsApp.Configuration;
using EventsApp.DAL.Interfaces;
using EventsApp.Domain.Errors;
using EventsApp.Domain.Models.Auth;
using EventsApp.Domain.Models.RefreshTokens;
using FluentResults;
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

    // TODO: вынести в конфигурацию
    private const int LifeTimeInMinutes = 30;

    public AuthService(
        IUserRepository userRepository, 
        IPasswordHashService passwordHashService,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<AuthSettings> authSettings, 
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
        _authSettings = authSettings.Value;
    }

    public async Task<Result<AuthTokenModel>> SignIn(SignInModel signInModel)
    {
        var userSignInDetails = await _userRepository.GetByEmailAsync(signInModel.Email);

        if (userSignInDetails is null) {
            return Result.Fail(new UserWithEmailNotFoundError(signInModel.Email));
        }

        if (!_passwordHashService.Verify(signInModel.Password, userSignInDetails.PasswordHash)) {
            return Result.Fail(new Error("Неверный пароль"));
        }

        if (await _refreshTokenRepository.GetByUserIdAsync(userSignInDetails.Id) is not null) {
            return Result.Fail(new Error($"У пользователя с email " +
                    $"{userSignInDetails.Email} есть действующий refresh token"));
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
        
        
        _ = await _refreshTokenRepository.AddAsync(refreshTokenModel);
        
        return new AuthTokenModel
        {
            AuthAccessTokenModel = authAccessTokenModel,
            RefreshToken = refreshToken
        };
    }

    public async Task<Result<AuthTokenModel>> GetNewTokensPairAsync(string refreshToken)
    {
        var refreshTokenModel = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (refreshTokenModel is null) {
            return Result.Fail(new Error($"Токен {refreshToken} не найден."));
        }

        var userDetails = await _userRepository.GetByIdAsync(refreshTokenModel.UserId);
        if (userDetails is null) {
            return Result.Fail(new Error($"Пользователь с Id {refreshTokenModel.UserId} не найден."));
        }
        
        // сгенерируем новую пару токенов
        var authAccessTokenModel = AuthTokenGenerator.GenerateAccessToken(new GenerateTokenPayload
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
        _ = await _refreshTokenRepository.DeleteByIdAsync(refreshTokenModel.Id);
        _ = await _refreshTokenRepository.AddAsync(newRefreshTokenModel);
        
        return new AuthTokenModel
        {
            AuthAccessTokenModel = authAccessTokenModel,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var refreshTokenModel = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (refreshTokenModel is null)
        {
            _logger.LogInformation("Refresh token не найден");
            return false;
        }
        
        _ = await _refreshTokenRepository.DeleteByIdAsync(refreshTokenModel.Id);
        _logger.LogInformation("Refresh token {token} успешно удален", refreshTokenModel.Token);
        return true;
    }
}
