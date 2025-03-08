using EventsApp.Domain.Models.Auth;

namespace EventsApp.BLL.Interfaces.Auth;

public interface IAuthService
{
    public Task<AuthTokenModel> SignIn(SignInModel signInModel, CancellationToken cancellationToken);
    
    public Task<AuthTokenModel> GetNewTokensPairAsync(string refreshToken, CancellationToken cancellationToken);
    
    public Task LogoutAsync(string refreshToken, CancellationToken cancellationToken);
}