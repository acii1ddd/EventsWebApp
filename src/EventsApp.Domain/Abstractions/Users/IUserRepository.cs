using EventsApp.Domain.Models.Participants;

namespace EventsApp.Domain.Abstractions.Users;

public interface IUserRepository : IRepository<UserModel>
{
    public Task<UserModel?> GetByEmailAsync(string email);
}