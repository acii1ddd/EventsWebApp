using EventsApp.Domain.Models.Events;
using FluentResults;

namespace EventsApp.Domain.Abstractions.Events;

public interface IEventService
{
    public Task<List<EventModel>> GetAllAsync();

    public Task<EventModel?> GetByIdAsync(Guid eventId);
    
    public Task<EventModel?> GetByNameAsync(string name);

    public Task<Result<EventModel>> AddAsync(EventModel eventModel, Stream fileStream, string fileName, string mimeType,
        Guid userId);
}
