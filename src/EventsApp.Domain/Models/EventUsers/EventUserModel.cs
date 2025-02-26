using EventsApp.Domain.Models.Events;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.Domain.Models.EventUsers;

public class EventUserModel
{
    public Guid EventId { get; set; }

    public EventModel Event { get; set; } = null!;
    
    public Guid UserId { get; set; }
 
    public UserModel User { get; set; } = null!;
    
    public DateTime RegisteredAt { get; set; }
}