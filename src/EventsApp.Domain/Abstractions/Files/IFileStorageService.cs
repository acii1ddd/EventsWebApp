using EventsApp.Domain.Models;

namespace EventsApp.Domain.Abstractions.Files;

public interface IFileStorageService
{
    public Task<ImageFileModel> UploadAsync(Stream fileStream, string fileName, string mimeType, Guid eventId);
}