using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshTokenEntity> AddAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return refreshToken;
    }

    public async Task<RefreshTokenEntity> DeleteAsync(RefreshTokenEntity refreshToken, CancellationToken cancellationToken)
    {
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);
        return refreshToken;
    }

    public async Task<RefreshTokenEntity?> GetByTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == refreshToken, cancellationToken);
    }
    
    public async Task<RefreshTokenEntity?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken: cancellationToken);
    }
}
