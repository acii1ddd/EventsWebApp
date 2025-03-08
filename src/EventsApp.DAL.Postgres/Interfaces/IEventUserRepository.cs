using EventsApp.DAL.Entities;

namespace EventsApp.DAL.Interfaces;

public interface IEventUserRepository
{
    public Task<EventUserEntity?> GetByEventAndUserIdAsync(Guid eventId, Guid userId, CancellationToken cancellationToken);
    
    public Task<EventUserEntity> AddAsync(EventUserEntity eventUserModel, CancellationToken cancellationToken);

    public Task<EventUserEntity> DeleteEventUserAsync(EventUserEntity eventUserEntity, CancellationToken cancellationToken);
}