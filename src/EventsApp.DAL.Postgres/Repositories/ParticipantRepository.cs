using AutoMapper;
using EventsApp.DAL.Context;
using EventsApp.Domain.Abstractions.Participants;
using EventsApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ParticipantRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<PaginatedList<ParticipantModel>> GetAllAsync(int pageIndex, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<ParticipantModel?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ParticipantModel> AddAsync(ParticipantModel entity)
    {
        throw new NotImplementedException();
    }

    public Task<ParticipantModel?> UpdateAsync(ParticipantModel newEntity)
    {
        throw new NotImplementedException();
    }

    public Task<ParticipantModel?> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ParticipantModel?> GetByEmailAsync(string email)
    {
        return _mapper.Map<ParticipantModel>(
            await _context.Participants
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email == email)
        );
    }
}