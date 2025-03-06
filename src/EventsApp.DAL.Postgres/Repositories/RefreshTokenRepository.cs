using AutoMapper;
using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.DAL.Interfaces;
using EventsApp.Domain.Models.RefreshTokens;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RefreshTokenRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RefreshTokenModel> AddAsync(RefreshTokenModel refreshTokenModel)
    {
        var entity = _mapper.Map<RefreshTokenEntity>(refreshTokenModel);
        
        await _context.RefreshTokens.AddAsync(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<RefreshTokenModel>(entity);
    }

    public async Task<RefreshTokenModel?> DeleteByIdAsync(Guid id)
    {
        var entity = await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            return null;
        }
        
        _context.RefreshTokens.Remove(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<RefreshTokenModel>(entity);
    }

    public async Task<RefreshTokenModel?> GetByTokenAsync(string refreshToken)
    {
        return _mapper.Map<RefreshTokenModel>(
            await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Token == refreshToken)
        );
    }
    
    public async Task<RefreshTokenModel?> GetByUserIdAsync(Guid userId)
    {
        return _mapper.Map<RefreshTokenModel>(
            await _context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId)
        );
    }
}
