using AutoMapper;
using EventsApp.DAL.Context;
using EventsApp.Domain.Abstractions.Users;
using EventsApp.Domain.Models;
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

    public Task<PaginatedList<UserModel>> GetAllAsync(int pageIndex, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<UserModel?> GetByIdAsync(Guid id)
    {
        return _mapper.Map<UserModel>(
            await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id)
        );
    }

    public Task<UserModel> AddAsync(UserModel entity)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel?> UpdateAsync(UserModel newEntity)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel?> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
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