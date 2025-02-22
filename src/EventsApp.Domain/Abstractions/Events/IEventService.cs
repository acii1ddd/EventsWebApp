using EventsApp.Domain.Models;

namespace EventsApp.Domain.Abstractions.Events;

public interface IEventService
{
    public Task<List<EventModel>> GetAllAsync();
}
