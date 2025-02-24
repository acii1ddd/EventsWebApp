using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;

namespace EventsApp.Domain.Abstractions.Events;

public interface IEventRepository : IRepository<EventModel>
{
    public Task<EventModel?> GetByNameAsync(string name);
    
    public Task<PaginatedList<EventModel>> GetByFilterAsync(DateTime? minDate, string? location, 
        string? category, int pageIndex, int pageSize);
}
