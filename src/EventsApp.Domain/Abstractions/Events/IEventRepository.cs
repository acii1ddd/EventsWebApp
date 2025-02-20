using EventsApp.Domain.Models;

namespace EventsApp.Domain.Abstractions.Events;

public interface IEventRepository : IRepository<EventModel>
{
    // crud
}