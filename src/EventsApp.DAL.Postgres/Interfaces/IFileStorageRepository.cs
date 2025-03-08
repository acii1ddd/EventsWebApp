using EventsApp.DAL.Entities;

namespace EventsApp.DAL.Interfaces;

public interface IFileStorageRepository
{
    public Task<List<ImageFileEntity>> GetAllAsync(CancellationToken cancellationToken);
    
    public Task<ImageFileEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    public Task<ImageFileEntity?> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
    
    public Task<ImageFileEntity> AddAsync(ImageFileEntity imageFile, CancellationToken cancellationToken);
    
    public Task<ImageFileEntity> UpdateAsync(ImageFileEntity imageFile, CancellationToken cancellationToken);

    public Task<ImageFileEntity> DeleteAsync(ImageFileEntity imageFile, CancellationToken cancellationToken);
}