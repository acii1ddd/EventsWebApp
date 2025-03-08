using EventsApp.DAL.Entities;

namespace EventsApp.DAL.Interfaces;

public interface IRefreshTokenRepository
{
    public Task<RefreshTokenEntity> AddAsync(RefreshTokenEntity refreshTokenModel, CancellationToken cancellationToken);
    
    public Task<RefreshTokenEntity> DeleteAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken);
    
    public Task<RefreshTokenEntity?> GetByTokenAsync(string refreshToken, CancellationToken cancellationToken);

    public Task<RefreshTokenEntity?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}