namespace EventsApp.API.Contracts.Events;

public class GetEventByFilterRequest
{
    public DateTime? Date { get; init; }
    
    public string? Location { get; init; } = string.Empty;
    
    public string? Category { get; init; } = string.Empty;
    
    public GetEventByPageRequest GetEventByPageRequest { get; init; } = null!;
}
