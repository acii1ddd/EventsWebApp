namespace EventsApp.DAL.Entities;

public class EventUserEntity
{
    public Guid EventId { get; set; }

    public EventEntity Event { get; set; } = null!;
    
    public Guid UserId { get; set; }
 
    public UserEntity User { get; set; } = null!;
    
    public DateTime RegisteredAt { get; set; }
}