namespace EventsApp.API.Contracts.Auth;

public class SignInRequest
{
    public string Email { get; init; } = string.Empty;
    
    public string Password { get; init; } = string.Empty;
}