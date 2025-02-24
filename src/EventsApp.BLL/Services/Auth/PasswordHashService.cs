using EventsApp.Domain.Abstractions.Auth;

namespace EventsApp.BLL.Services.Auth;

public class PasswordHashService : IPasswordHashService
{
    public bool Verify(string password, string passwordHash) 
        => BCrypt.Net.BCrypt.Verify(password, passwordHash);

    public string HashPassword(string password) 
        => BCrypt.Net.BCrypt.HashPassword(password);
}