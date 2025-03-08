namespace EventsApp.API.Contracts.Events.Requests;

public class UpdateEventWithImageRequest
{
    public UpdateEventRequest EventData { get; init; } = null!;
    
    public IFormFile ImageFile { get; init; } = null!;
}