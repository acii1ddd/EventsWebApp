using AutoMapper;
using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Models;
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
    public async Task<List<EventModel>> GetAllAsync()
    {
        return _mapper.Map<List<EventModel>>(
            await _context.Events
                .AsNoTracking()
                .ToListAsync()
        );
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
                .Where(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
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
    
    // подумать про patch частичное изменение

    /// <summary>
    /// Удаление события
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<EventModel?> DeleteByIdAsync(Guid id)
    {
        var entity = _context.Events
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (entity is null)
        {
            return null;
        }
        
        _context.Events.Remove(entity);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<EventModel>(entity);
    }
    
    // обобщить
    
    /// <summary>
    ///  Получение событий по критериям
    /// </summary>
    /// <param name="minDate"></param>
    /// <param name="location"></param>
    /// <param name="category"></param>
    public async Task<List<EventModel>> GetByFilterAsync(DateTime? minDate, string? location, string? category)
    {
        var query = _context.Events.AsNoTracking();

        if (minDate.HasValue)
        {
            query = query.Where(x => x.StartDate >= minDate);
        }
        
        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(x => x.Location.Contains(location));
        }
        
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(x => x.Category.Contains(category));
        }

        return _mapper.Map<List<EventModel>>(
            await query.ToListAsync()
        );
    }
}