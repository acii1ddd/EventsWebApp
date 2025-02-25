namespace EventsApp.Domain.Models.Auth;

public class AuthTokenModel
{
    public required AuthAccessTokenModel AuthAccessTokenModel { get; set; }
    
    public required string RefreshToken { get; set; }
}

public class AuthAccessTokenModel
{
    public required Guid UserId { get; set; }
    
    public required UserRole UserRole { get; set; }
    
    public required string AccessToken { get; set; }
    
    public required DateTime Expires { get; set; }
}