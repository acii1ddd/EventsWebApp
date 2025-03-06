using EventsApp.Domain.Models.Participants;

namespace EventsApp.DAL.Interfaces;

public interface IUserRepository
{
    public Task<UserModel?> GetByIdAsync(Guid id);
    
    public Task<UserModel?> GetByEmailAsync(string email);
}