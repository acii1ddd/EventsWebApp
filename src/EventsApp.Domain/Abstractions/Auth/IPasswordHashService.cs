namespace EventsApp.Domain.Abstractions.Auth;

public interface IPasswordHashService
{
    public bool Verify(string password, string passwordHash);
    
    public string HashPassword(string password);
}