using EventsApp.Domain.Models.Participants;

namespace EventsApp.Domain.Models.RefreshTokens;

public class RefreshTokenModel
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

    public UserModel User { get; set; } = null!;
}