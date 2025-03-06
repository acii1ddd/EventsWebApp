using EventsApp.Domain.Models.EventUsers;

namespace EventsApp.DAL.Interfaces;

public interface IEventUserRepository
{
    public Task<EventUserModel?> GetByEventAndUserIdAsync(Guid eventId, Guid userId);
    
    public Task<EventUserModel> AddAsync(EventUserModel eventUserModel);
    
    public Task<EventUserModel?> DeleteByEventAndUserIdAsync(Guid eventId, Guid userId);
}