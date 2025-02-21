using EventsApp.Domain.Models;

namespace EventsApp.Domain.Abstractions.Events;

public interface IEventRepository : IRepository<EventModel>
{
    public Task<EventModel?> GetByNameAsync(string name);
}
