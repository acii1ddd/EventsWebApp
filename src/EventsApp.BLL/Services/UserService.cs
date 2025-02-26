using System.Diagnostics;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Abstractions.EventUsers;
using EventsApp.Domain.Abstractions.Users;
using EventsApp.Domain.Errors;
using EventsApp.Domain.Models.EventUsers;
using EventsApp.Domain.Models.Participants;
using FluentResults;

namespace EventsApp.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IEventUserRepository _eventUserRepository;

    public UserService(IUserRepository userRepository, IEventRepository eventRepository, IEventUserRepository eventUserRepository)
    {
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _eventUserRepository = eventUserRepository;
    }

    public async Task<Result<bool>> RegisterToEventAsync(Guid eventId, Guid userId)
    {
        var userModel = await _userRepository.GetByIdAsync(userId);
        if (userModel == null) {
            return Result.Fail(new UserWithIdNotFoundError(userId));
        }
        
        var eventModel = await _eventRepository.GetByIdAsync(eventId);
        if (eventModel == null) {
            return Result.Fail(new EventWithIdNotFoundError(eventId));
        }

        var registered = await _eventUserRepository.GetByEventAndUserIdAsync(eventId, userId);
        if (registered != null) return false;

        // подписываем пользователя на событие
        var eventUserModel = new EventUserModel
        {
            EventId = eventId,
            UserId = userId,
            RegisteredAt = DateTime.UtcNow
        };
        _ = await _eventUserRepository.AddAsync(eventUserModel);
        return true;
    }
    
    public async Task<List<UserModel>> GetEventParticipantsAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }
}
