using AutoMapper;
using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Abstractions.EventUsers;
using EventsApp.Domain.Models.EventUsers;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class EventUserRepository : IEventUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EventUserRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EventUserModel?> GetByEventAndUserIdAsync(Guid eventId, Guid userId)
    {
        return _mapper.Map<EventUserModel>(
            await _context.EventUsers
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Event)
                .FirstOrDefaultAsync(x => x.EventId == eventId && x.UserId == userId)
        );
    }

    public async Task<EventUserModel> AddAsync(EventUserModel eventUserModel)
    {
        var entity = _mapper.Map<EventUserEntity>(eventUserModel);
        await _context.EventUsers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<EventUserModel>(entity);
    }
}
