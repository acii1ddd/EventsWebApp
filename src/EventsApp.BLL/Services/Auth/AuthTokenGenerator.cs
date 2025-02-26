using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EventsApp.Configuration;
using EventsApp.Domain.Models.Auth;
using Microsoft.IdentityModel.Tokens;

namespace EventsApp.BLL.Services.Auth;

public static class AuthTokenGenerator
{
    public static AuthAccessTokenModel GenerateAccessToken(
        GenerateTokenPayload payload, 
        AuthSettings authSettings)
    {
        // дата окончания срока жизни токена
        var expires = DateTime.UtcNow.AddMinutes(authSettings.Lifetime);

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new("userId", payload.UserId.ToString()),
            new(ClaimTypes.Role, payload.UserRole.ToString())
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = authSettings.Issuer,
            Audience = authSettings.Audience,
            Expires = expires,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Secret)),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var securityToken = tokenHandler.CreateToken(descriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return new AuthAccessTokenModel
        {
            UserId = payload.UserId,
            UserRole = payload.UserRole,
            AccessToken = token,
            Expires = expires
        };
    }

    public static string GenerateRefreshToken()
    {
        var bytes = new byte[32];

        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}