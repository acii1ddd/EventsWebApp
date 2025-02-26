using AutoMapper;
using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public EventRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Получение списка всех событий
    /// </summary>
    /// <returns>Список событий либо пустой список</returns>
    public async Task<PaginatedList<EventModel>> GetAllAsync(int pageIndex, int pageSize)
    {
        var query = _context.Events
            .AsNoTracking()
            .OrderBy(x => x.StartDate);
        
        var totalRecords = await query.CountAsync(); 
        
        var items = _mapper.Map<List<EventModel>>(await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Include(x => x.ImageFile)
            .ToListAsync()
        );
        
        var totalPages = (int) Math.Ceiling(totalRecords / (double)pageSize);
        return new PaginatedList<EventModel>(items, pageIndex, totalPages);
    }

    /// <summary>
    /// Получение события по Id
    /// </summary>
    /// <param name="id"></param>
    public async Task<EventModel?> GetByIdAsync(Guid id)
    {
        return _mapper.Map<EventModel>(
            await _context.Events
                .AsNoTracking()
                .Include(x => x.ImageFile)
                .Include(x => x.EventUsers)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id)
        );
    }
    
    /// <summary>
    /// Получение события по его названию
    /// </summary>
    /// <param name="name"></param>
    public async Task<EventModel?> GetByNameAsync(string name)
    {
        return _mapper.Map<EventModel>(
            await _context.Events
                .AsNoTracking()
                .Where(x => x.Name.ToLower() == name.ToLower())
                .Include(x => x.ImageFile)
                .FirstOrDefaultAsync()
        );
    }
    
    /// <summary>
    /// Добавление нового события
    /// </summary>
    /// <param name="eventModel"></param>
    public async Task<EventModel> AddAsync(EventModel eventModel)
    {
        var eventEntity = _mapper.Map<EventEntity>(eventModel);
        
        await _context.Events.AddAsync(eventEntity);
        await _context.SaveChangesAsync();
        return _mapper.Map<EventModel>(eventEntity);
    }

    /// <summary>
    /// Изменение информации о событии
    /// </summary>
    /// <param name="newEventModel"></param>
    public async Task<EventModel?> UpdateAsync(EventModel newEventModel)
    {
        var entity = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == newEventModel.Id);

        if (entity is null)
        {
            return null;
        }
        
        var eventEntity = _mapper.Map<EventEntity>(newEventModel);
        _context.Events.Update(eventEntity);
        await _context.SaveChangesAsync();
        return _mapper.Map<EventModel>(eventEntity);
    }
    
    /// <summary>
    /// Удаление события
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<EventModel?> DeleteByIdAsync(Guid id)
    {
        var entity = _context.Events
            .AsNoTracking()
            .Include(x => x.ImageFile)
            .FirstOrDefault(x => x.Id == id);

        if (entity is null)
        {
            return null;
        }
        
        _context.Events.Remove(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<EventModel>(entity);
    }

    /// <summary>
    ///  Получение событий по критериям
    /// </summary>
    /// <param name="minDate"></param>
    /// <param name="location"></param>
    /// <param name="category"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    public async Task<PaginatedList<EventModel>> GetByFilterAsync(DateTime? minDate, string? location, 
        string? category, int pageIndex, int pageSize)
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
        
        var totalRecords = await query.CountAsync();
        
        // округляем в большую сторону, чтобы забрать последнюю неполную страницу
        var totalPages = (int) Math.Ceiling(totalRecords / (double)pageSize);

        var items = _mapper.Map<List<EventModel>>(await query
            .OrderBy(x => x.StartDate)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Include(x => x.ImageFile)
            .ToListAsync()
        );
        return new PaginatedList<EventModel>(items, pageIndex, totalPages);
    }
}