namespace EventsApp.Domain.Models.Auth;

public class AuthTokenModel
{
    public required AuthAccessTokenModel AuthAccessTokenModel { get; set; }
    
    public required string RefreshToken { get; set; }
}

public class AuthAccessTokenModel
{
    public required Guid UserId { get; set; }
    
    public required string Token { get; set; }
    
    public required DateTime Expires { get; set; }
}