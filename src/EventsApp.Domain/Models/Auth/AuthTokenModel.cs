namespace EventsApp.Domain.Models.Auth;

public class AuthTokenModel
{
    public required AuthAccessTokenModel AuthAccessTokenModel { get; set; }
    
    public required string RefreshToken { get; set; }
}