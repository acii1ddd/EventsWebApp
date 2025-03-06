using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;

namespace EventsApp.DAL.Interfaces;

public interface IEventRepository
{
    public Task<PaginatedList<EventModel>> GetAllAsync(int pageIndex, int pageSize);
    
    public Task<EventModel?> GetByIdAsync(Guid id);
    
    public Task<EventModel?> GetByNameAsync(string name);
    
    public Task<EventModel> AddAsync(EventModel entity);

    public Task<EventModel?> UpdateAsync(EventModel newEntity);

    public Task<EventModel?> DeleteByIdAsync(Guid id);
    
    public Task<PaginatedList<EventModel>> GetByFilterAsync(DateTime? minDate, string? location, 
        string? category, int pageIndex, int pageSize);
}
