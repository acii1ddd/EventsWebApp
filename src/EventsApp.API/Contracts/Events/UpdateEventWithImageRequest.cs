namespace EventsApp.API.Contracts.Events;

public class UpdateEventWithImageRequest
{
    public UpdateEventRequest EventData { get; init; } = null!;
    
    public IFormFile ImageFile { get; init; } = null!;
}