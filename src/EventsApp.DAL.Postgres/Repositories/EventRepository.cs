using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.DAL.Interfaces;
using EventsApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;
    
    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получение списка всех событий
    /// </summary>
    /// <returns>Список событий либо пустой список</returns>
    public async Task<PaginatedList<EventEntity>> GetAllAsync(int pageIndex, int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _context.Events
            .AsNoTracking()
            .OrderBy(x => x.StartDate);
        
        var totalRecords = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Include(x => x.ImageFile)
            .ToListAsync(cancellationToken);
        
        var totalPages = (int) Math.Ceiling(totalRecords / (double)pageSize);
        return new PaginatedList<EventEntity>(items, pageIndex, totalPages);
    }

    /// <summary>
    /// Получение события по Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    public async Task<EventEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Events
            .AsNoTracking()
            .Include(x => x.ImageFile)
            .Include(x => x.EventUsers)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>
    /// Получение события по его названию
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    public async Task<EventEntity?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Events
            .AsNoTracking()
            .Where(x => x.Name.ToLower() == name.ToLower())
            .Include(x => x.ImageFile)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Добавление нового события
    /// </summary>
    /// <param name="eventEntity"></param>
    /// <param name="cancellationToken"></param>
    public async Task<EventEntity> AddAsync(EventEntity eventEntity, CancellationToken cancellationToken)
    {
        await _context.Events.AddAsync(eventEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return eventEntity;
    }

    /// <summary>
    /// Изменение информации о событии
    /// </summary>
    /// <param name="newEventEntity"></param>
    /// <param name="cancellationToken"></param>
    public async Task<EventEntity> UpdateAsync(EventEntity newEventEntity, CancellationToken cancellationToken)
    {
        _context.Events.Update(newEventEntity);
        await _context.SaveChangesAsync(cancellationToken);
        return newEventEntity;
    }

    /// <summary>
    /// Удаление события
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<EventEntity> DeleteAsync(EventEntity entity, CancellationToken cancellationToken)
    {
        _context.Events.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    ///  Получение событий по критериям
    /// </summary>
    /// <param name="minDate"></param>
    /// <param name="location"></param>
    /// <param name="category"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    public async Task<PaginatedList<EventEntity>> GetByFilterAsync(DateTime? minDate, string? location, 
        string? category, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Events.AsNoTracking();

        if (minDate.HasValue)
        {
            query = query.Where(x => x.StartDate >= minDate.Value.ToUniversalTime());
        }
        
        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(x => x.Location.ToLower().Contains(location.ToLower()));
        }
        
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(x => x.Category.ToLower().Contains(category.ToLower()));
        }
        
        var totalRecords = await query.CountAsync(cancellationToken);
        
        // округляем в большую сторону, чтобы забрать последнюю неполную страницу
        var totalPages = (int) Math.Ceiling(totalRecords / (double)pageSize);

        var items = await query
            .OrderBy(x => x.StartDate)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Include(x => x.ImageFile)
            .ToListAsync(cancellationToken);
        
        return new PaginatedList<EventEntity>(items, pageIndex, totalPages);
    }
}