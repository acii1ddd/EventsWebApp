namespace EventsApp.Domain.Models.Auth;

public class SignInModel
{
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
}