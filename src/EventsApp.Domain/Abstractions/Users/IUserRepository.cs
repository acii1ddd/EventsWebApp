using EventsApp.Domain.Models.Participants;

namespace EventsApp.Domain.Abstractions.Users;

public interface IUserRepository
{
    public Task<UserModel?> GetByIdAsync(Guid id);
    
    public Task<UserModel?> GetByEmailAsync(string email);
}