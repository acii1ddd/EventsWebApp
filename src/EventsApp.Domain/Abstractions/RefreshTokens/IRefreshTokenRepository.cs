using EventsApp.Domain.Models.RefreshTokens;

namespace EventsApp.Domain.Abstractions.RefreshTokens;

public interface IRefreshTokenRepository
{
    public Task<RefreshTokenModel> AddAsync(RefreshTokenModel refreshTokenModel);
    
    public Task<RefreshTokenModel?> DeleteByIdAsync(Guid id);
    
    public Task<RefreshTokenModel?> GetByTokenAsync(string refreshToken);

    public Task<RefreshTokenModel?> GetByUserIdAsync(Guid userId);
}