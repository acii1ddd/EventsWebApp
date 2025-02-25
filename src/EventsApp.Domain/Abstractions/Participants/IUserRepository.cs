using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Participants;

namespace EventsApp.Domain.Abstractions.Participants;

public interface IUserRepository : IRepository<UserModel>
{
    public Task<UserModel?> GetByEmailAsync(string email);
}