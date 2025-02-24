using EventsApp.Domain.Models.RefreshTokens;

namespace EventsApp.Domain.Abstractions.RefreshTokens;

public interface IRefreshTokenRepository
{
    public Task<RefreshTokenModel> AddAsync(RefreshTokenModel refreshTokenModel);
    
    public Task<RefreshTokenModel?> DeleteByIdAsync(Guid id);
}