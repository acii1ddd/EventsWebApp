namespace EventsApp.Configuration;

public class AuthSettings
{
    public string Issuer { get; set; } = string.Empty;
    
    public string Audience { get; set; } = string.Empty;
    
    public int Lifetime { get; set; }
    
    public string Secret { get; set; } = string.Empty;
}
