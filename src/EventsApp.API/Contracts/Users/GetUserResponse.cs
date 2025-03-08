using EventsApp.Domain.Models.Auth;

namespace EventsApp.API.Contracts.Users;

public class GetUserResponse
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public string Surname { get; init; } = string.Empty;
    
    public DateTime BirthDate { get; init; }
    
    public DateTime EventRegistrationDate { get; init; }
    
    public string Email { get; init; } = string.Empty;
    
    public string PasswordHash { get; init; } = string.Empty;
    
    public UserRole Role { get; init; }
}