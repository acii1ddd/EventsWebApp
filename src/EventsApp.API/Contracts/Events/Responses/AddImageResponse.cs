namespace EventsApp.API.Contracts.Events.Responses;

public class AddImageResponse
{
    public Guid EventId { get; init; }
        
    public string ImageUrl { get; init; } = string.Empty;
}