namespace EventsApp.API.Contracts.Events;

public class AddImageRequest
{
    public IFormFile ImageFile { get; set; } = null!;
}