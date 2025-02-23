namespace EventsApp.API.Contracts.Events;

public class AddEventWithImageRequest
{
    public AddEventRequest EventData { get; init; } = null!;

    public IFormFile? ImageFile { get; init; } = null!;
}