using EventsApp.Configuration;
using EventsApp.Domain.Abstractions.Auth;
using EventsApp.Domain.Abstractions.Participants;
using EventsApp.Domain.Abstractions.RefreshTokens;
using EventsApp.Domain.Errors;
using EventsApp.Domain.Models.Auth;
using EventsApp.Domain.Models.RefreshTokens;
using FluentResults;
using Microsoft.Extensions.Options;

namespace EventsApp.BLL.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly AuthSettings _authSettings;

    public AuthService(
        IParticipantRepository participantRepository, 
        IPasswordHashService passwordHashService,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<AuthSettings> authSettings)
    {
        _participantRepository = participantRepository;
        _passwordHashService = passwordHashService;
        _refreshTokenRepository = refreshTokenRepository;
        _authSettings = authSettings.Value;
    }

    public async Task<Result<AuthTokenModel>> SignIn(SignInModel signInModel)
    {
        var userSignInDetails = await _participantRepository.GetByEmailAsync(signInModel.Email);

        if (userSignInDetails is null) {
            return Result.Fail(new UserWithEmailNotFoundError(signInModel.Email));
        }

        if (!_passwordHashService.Verify(signInModel.Password, userSignInDetails.PasswordHash)) {
            return Result.Fail(new Error("Пароль неверный"));
        }
        
        // сгенерируем access token
        var authAccessTokenModel = AuthTokenGenerator.GenerateAccessToken(new GenerateTokenPayload
        {
            UserId = userSignInDetails.Id
        }, _authSettings);
        
        // сгенерируем refresh token
        var refreshToken = AuthTokenGenerator.GenerateRefreshToken();
        
        // сохраняем refresh token в бд
        
        // ВЫНЕСТИ В КОНФИГУРАЦИЮ (сколько будем хранить в куках рефреш)
        var expiryDate = DateTime.Now.AddMinutes(30);
        
        var refreshTokenModel = new RefreshTokenModel
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            ExpiryDate = expiryDate,
            CreatedDate = DateTime.UtcNow,
            ParticipantId = userSignInDetails.Id
        };
        
        _ = await _refreshTokenRepository.AddAsync(refreshTokenModel);
        
        return new AuthTokenModel
        {
            AuthAccessTokenModel = authAccessTokenModel,
            RefreshToken = refreshToken
        };
    }
}
