using EventsApp.DAL.Entities;

namespace EventsApp.DAL.Interfaces;

public interface IUserRepository
{
    public Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    public Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    public Task AddAsync(UserEntity userEntity, CancellationToken cancellationToken);
}