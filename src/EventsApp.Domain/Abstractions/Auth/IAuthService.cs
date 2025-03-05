using System.Threading.Tasks;
using EventsApp.Domain.Models.Auth;
using FluentResults;

namespace EventsApp.Domain.Abstractions.Auth;

public interface IAuthService
{
    public Task<Result<AuthTokenModel>> SignIn(SignInModel signInModel);
    
    public Task<Result<AuthTokenModel>> GetNewTokensPairAsync(string refreshToken);
    
    public Task<bool> LogoutAsync(string refreshToken);
}