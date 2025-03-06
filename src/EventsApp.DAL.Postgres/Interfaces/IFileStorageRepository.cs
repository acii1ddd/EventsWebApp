using EventsApp.Domain.Models.Images;

namespace EventsApp.DAL.Interfaces;

public interface IFileStorageRepository
{

    public Task<ImageFileModel?> GetByIdAsync(Guid id);
    
    public Task<ImageFileModel?> GetByEventIdAsync(Guid eventId);
    
    public Task<ImageFileModel> AddAsync(ImageFileModel imageFile);
    
    public Task<ImageFileModel?> UpdateAsync(ImageFileModel imageFile);

    public Task<ImageFileModel?> DeleteByIdAsync(Guid id);
}