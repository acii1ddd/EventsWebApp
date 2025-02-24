using EventsApp.Domain.Models;

namespace EventsApp.Domain.Abstractions.Participants;

public interface IParticipantRepository : IRepository<ParticipantModel>
{
    public Task<ParticipantModel?> GetByEmailAsync(string email);
}