using System;
using System.Threading.Tasks;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Images;

namespace EventsApp.Domain.Abstractions.Files;

public interface IFileStorageRepository
{

    public Task<ImageFileModel?> GetByIdAsync(Guid id);
    
    public Task<ImageFileModel?> GetByEventIdAsync(Guid eventId);
    
    public Task<ImageFileModel> AddAsync(ImageFileModel imageFile);
    
    public Task<ImageFileModel?> UpdateAsync(ImageFileModel imageFile);

    public Task<ImageFileModel?> DeleteByIdAsync(Guid id);
}