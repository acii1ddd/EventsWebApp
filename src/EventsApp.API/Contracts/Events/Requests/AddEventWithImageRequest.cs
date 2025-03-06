namespace EventsApp.API.Contracts.Events.Requests;

public class AddEventWithImageRequest
{
    public AddEventRequest EventData { get; init; } = null!;

    public IFormFile? ImageFile { get; init; } = null!;
}