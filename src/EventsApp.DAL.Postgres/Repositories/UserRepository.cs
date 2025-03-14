using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

    public async Task AddAsync(UserEntity userEntity, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(userEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}