using EventsApp.Domain.Models;

namespace EventsApp.Domain.Abstractions.Files;

public interface IFileStorageRepository
{
    public Task<ImageFileModel> AddAsync(ImageFileModel imageFile);
}