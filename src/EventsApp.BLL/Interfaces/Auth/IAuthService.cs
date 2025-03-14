using EventsApp.Domain.Models.Auth;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.BLL.Interfaces.Auth;

public interface IAuthService
{
    public Task<AuthTokenModel> SignIn(SignInModel signInModel, CancellationToken cancellationToken);
    
    public Task<AuthTokenModel> GetNewTokensPairAsync(string refreshToken, CancellationToken cancellationToken);
    
    public Task LogoutAsync(string refreshToken, CancellationToken cancellationToken);
    
    public Task SignUpAsync(UserModel userModel, CancellationToken cancellationToken);
}