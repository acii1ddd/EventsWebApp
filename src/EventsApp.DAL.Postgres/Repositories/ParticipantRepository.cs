using EventsApp.Domain.Abstractions.Participants;
using EventsApp.Domain.Models;

namespace EventsApp.DAL.Repositories;

public class ParticipantRepository : IParticipantRepository
{
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
}