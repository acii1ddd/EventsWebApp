using AutoMapper;
using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Abstractions.Files;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Images;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class FileStorageRepository : IFileStorageRepository
{
    private readonly ApplicationDbContext _context;
    private IMapper _mapper;

    public FileStorageRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<ImageFileModel>> GetAllAsync()
    {
        return _mapper.Map<List<ImageFileModel>>(
            await _context.ImageFiles
            .AsNoTracking()
            .ToListAsync()
        );
    }
    
    public async Task<ImageFileModel?> GetByIdAsync(Guid id)
    {
        return _mapper.Map<ImageFileModel>(
            await _context.ImageFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id)
        );
    }
    
    public async Task<ImageFileModel?> GetByEventIdAsync(Guid eventId)
    {
        return _mapper.Map<ImageFileModel>(
            await _context.ImageFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EventId == eventId)
        );
    }
    
    public async Task<ImageFileModel> AddAsync(ImageFileModel imageFile)
    {
        var imageFileEntity = _mapper.Map<ImageFileEntity>(imageFile);
        await _context.ImageFiles.AddAsync(imageFileEntity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<ImageFileModel>(imageFileEntity);
    }
    
    public async Task<ImageFileModel?> UpdateAsync(ImageFileModel imageFile)
    {
        var image = _context.ImageFiles
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == imageFile.Id);

        if (image is null)
        {
            return null;
        }
        
        var imageEntity = _mapper.Map<ImageFileEntity>(imageFile);
        _context.ImageFiles.Update(imageEntity);
        await _context.SaveChangesAsync();
        return _mapper.Map<ImageFileModel>(image);
    }
    
    public async Task<ImageFileModel?> DeleteByIdAsync(Guid id)
    {
        var image = _context.ImageFiles
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (image is null)
        {
            return null;
        }
        
        _context.ImageFiles.Remove(image);
        await _context.SaveChangesAsync();
        return _mapper.Map<ImageFileModel>(image);
    }
}