using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Models;

namespace EventsApp.BLL.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<List<EventModel>> GetAllAsync()
    {
        return await _eventRepository.GetAllAsync();
    }
}