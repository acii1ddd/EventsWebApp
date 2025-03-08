using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class FileStorageRepository : IFileStorageRepository
{
    private readonly ApplicationDbContext _context;

    public FileStorageRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ImageFileEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.ImageFiles
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
    }
    
    public async Task<ImageFileEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.ImageFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<ImageFileEntity?> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await _context.ImageFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EventId == eventId, cancellationToken);
    }
    
    public async Task<ImageFileEntity> AddAsync(ImageFileEntity imageFile, CancellationToken cancellationToken)
    {
        await _context.ImageFiles.AddAsync(imageFile, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return imageFile;
    }
    
    public async Task<ImageFileEntity> UpdateAsync(ImageFileEntity imageFile, CancellationToken cancellationToken)
    {
        _context.ImageFiles.Update(imageFile);
        await _context.SaveChangesAsync(cancellationToken);
        return imageFile;
    }
    
    public async Task<ImageFileEntity> DeleteAsync(ImageFileEntity imageFile, CancellationToken cancellationToken)
    {
        _context.ImageFiles.Remove(imageFile);
        await _context.SaveChangesAsync(cancellationToken);
        return imageFile;
    }
}