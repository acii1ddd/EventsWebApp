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
    
    /// <summary>
    /// Изображение события
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;
}
