using EventsApp.Configuration;
using EventsApp.Domain.Models.Auth;

namespace EventsApp.BLL.Services.Auth;

public static class AuthTokenGenerator
{
    public static AuthAccessTokenModel GenerateAccessToken(
        GenerateTokenPayload payload, 
        AuthSettings authSettings)
    {
        throw new NotImplementedException();
    }

    public static string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }
}