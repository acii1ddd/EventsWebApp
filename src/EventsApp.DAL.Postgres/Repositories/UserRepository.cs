using AutoMapper;
using EventsApp.DAL.Context;
using EventsApp.Domain.Abstractions.Users;
using EventsApp.Domain.Models.Participants;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserModel?> GetByIdAsync(Guid id)
    {
        return _mapper.Map<UserModel>(
            await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id)
        );
    }
    
    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        return _mapper.Map<UserModel>(
            await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email == email)
        );
    }
}