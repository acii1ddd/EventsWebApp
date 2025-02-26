using EventsApp.Domain.Models.EventUsers;
using EventsApp.Domain.Models.Images;

namespace EventsApp.Domain.Models.Events;

public class EventModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public string Location { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    public int MaxParticipants { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public ImageFileModel ImageFile { get; set; } = null!;
    
    public List<EventUserModel> EventUsers { get; set; } = [];
}
