namespace EventsApp.API.Contracts.Events.Requests;

public class AddImageRequest
{
    public IFormFile ImageFile { get; set; } = null!;
}