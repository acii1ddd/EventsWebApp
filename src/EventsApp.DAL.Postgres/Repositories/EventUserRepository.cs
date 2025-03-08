using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class EventUserRepository : IEventUserRepository
{
    private readonly ApplicationDbContext _context;

    public EventUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EventUserEntity?> GetByEventAndUserIdAsync(Guid eventId, Guid userId, CancellationToken cancellationToken)
    {
        return await _context.EventUsers
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Event)
            .FirstOrDefaultAsync(x => x.EventId == eventId && x.UserId == userId, cancellationToken);
    }

    public async Task<EventUserEntity> AddAsync(EventUserEntity eventUserEntity, CancellationToken cancellationToken)
    {
        await _context.EventUsers.AddAsync(eventUserEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return eventUserEntity;
    }

    public async Task<EventUserEntity> DeleteEventUserAsync(EventUserEntity eventUserEntity, CancellationToken cancellationToken)
    {
        _context.EventUsers.Remove(eventUserEntity);
        await _context.SaveChangesAsync(cancellationToken);
        return eventUserEntity;
    }
}
