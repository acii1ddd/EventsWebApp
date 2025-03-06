using EventsApp.Domain.Models.Auth;

namespace EventsApp.DAL.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Surname { get; set; } = string.Empty;
    
    public DateTime BirthDate { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public UserRole Role { get; set; }
    
    /// <summary>
    /// События этого участника
    /// </summary>
    // public List<EventEntity> Events { get; set; } = [];
    
    /// <summary>
    /// Refresh token участника
    /// </summary>
    public RefreshTokenEntity RefreshToken { get; set; } = null!;
    
    //public Guid RefreshTokenId { get; set; }
    
    public List<EventUserEntity> EventUsers { get; set; } = [];
}