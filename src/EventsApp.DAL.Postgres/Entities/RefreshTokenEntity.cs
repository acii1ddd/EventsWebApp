namespace EventsApp.DAL.Entities;

public class RefreshTokenEntity
{
    public Guid Id { get; set; }
    
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата окончания срока жизни токена
    /// </summary>
    public DateTime ExpiryDate { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    /// <summary>
    /// Владелец токена
    /// </summary>
    public Guid UserId { get; set; }

    public UserEntity User { get; set; } = null!;
}