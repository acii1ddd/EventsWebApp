using FluentResults;

namespace EventsApp.Domain.Abstractions.Users;

public interface IUserService
{
    public Task<Result<bool>> RegisterToEventAsync(Guid eventId, Guid userId);
}