using EventsApp.Domain.Models.Events;
using EventsApp.Domain.Models.EventUsers;

namespace EventsApp.Domain.Abstractions.EventUsers;

public interface IEventUserRepository
{
    public Task<EventUserModel?> GetByEventAndUserIdAsync(Guid eventId, Guid userId);
    
    public Task<EventUserModel> AddAsync(EventUserModel eventUserModel);
}