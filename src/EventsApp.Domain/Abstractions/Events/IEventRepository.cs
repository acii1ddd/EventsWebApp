using EventsApp.Domain.Models;

namespace EventsApp.Domain.Abstractions.Events;

public interface IEventRepository : IRepository<EventModel>
{
    public Task<EventModel?> GetByNameAsync(string name);

    public Task<List<EventModel>> GetByFilterAsync(DateTime? minDate, string? location, string? category);
}
