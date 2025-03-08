namespace EventsApp.API.Contracts.Events.Responses;

public class GetEventResponse
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public DateTime StartDate { get; init; }
    
    public string Location { get; init; } = string.Empty;
    
    public string Category { get; init; } = string.Empty;
    
    public int MaxParticipants { get; init; }
    
    /// <summary>
    /// Изображение события
    /// </summary>
    public string ImageUrl { get; init; } = string.Empty;
}