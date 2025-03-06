using EventsApp.BLL.Interfaces;
using EventsApp.DAL.Interfaces;
using EventsApp.Domain.Errors;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;
using EventsApp.Domain.Models.EventUsers;
using EventsApp.Domain.Models.Participants;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EventsApp.BLL.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IEventUserRepository _eventUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<EventService> _logger;
    
    public EventService(
        IEventRepository eventRepository, 
        IFileStorageService fileService, 
        ILogger<EventService> logger, 
        IUserRepository userRepository, 
        IEventUserRepository eventUserRepository)
    {
        _eventRepository = eventRepository;
        _fileStorageService = fileService;
        _logger = logger;
        _userRepository = userRepository;
        _eventUserRepository = eventUserRepository;
    }

    public async Task<PaginatedList<EventModel>> GetAllAsync(int pageIndex, int pageSize)
    {
        var events = await _eventRepository.GetAllAsync(pageIndex, pageSize);
    
        if (events.Items.Count == 0) {
            return events;
        }
    
        foreach (var eventModel in events.Items)
        {
            eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.ImageFile);
        }
        return events;
    }
    
    public async Task<EventModel?> GetByIdAsync(Guid eventId)
    {
        var eventModel = await _eventRepository.GetByIdAsync(eventId);
    
        if (eventModel is null) {
            return null;
        }
        
        eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.ImageFile);
        return eventModel;
    }

    public async Task<EventModel?> GetByNameAsync(string name)
    {
        var eventModel = await _eventRepository.GetByNameAsync(name);
        
        if (eventModel is null) {
            return null;
        }
        
        eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.ImageFile);
        return eventModel;
    }

    /// <summary>
    /// Добавление события с его изображением
    /// </summary>
    /// <param name="eventModel"></param>
    /// <param name="imageFile"></param>
    /// <returns>Добавленное событие и его изображение</returns>
    public async Task<Result<EventModel>> AddAsync(EventModel eventModel, IFormFile? imageFile)
    {
        try
        {
            var eventId = Guid.NewGuid();
            eventModel.Id = eventId;
            eventModel.StartDate = eventModel.StartDate.ToUniversalTime();
            
            var result = await _eventRepository.AddAsync(eventModel);
            
            // у события нет изображения
            if (imageFile is null) {
                return result;
            }
            
            // сохранение изображение события
            await using var fileStream = imageFile.OpenReadStream();

            var imageUrl = await _fileStorageService
                .UploadAsync(fileStream, imageFile.FileName, imageFile.ContentType, eventId);
            result.ImageUrl = imageUrl;
            
            _logger.LogInformation("Событие {eventName} успешно добавлено", eventModel.Name);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время добавления события");
            return Result.Fail(new Error(e.Message));
        }
    }
    
    public async Task<EventModel?> UpdateAsync(EventModel eventModel,
        IFormFile imageFile)
    {
        // сохраняем дату в utc
        eventModel.StartDate = eventModel.StartDate.ToUniversalTime();
        var updatedEventModel = await _eventRepository.UpdateAsync(eventModel);

        if (updatedEventModel is null) {
            return null;
        }

        // поток для обновления изображения в minio
        await using var fileStream = imageFile.OpenReadStream();
        
        // обновляем изображение события
        var imageUrl = await _fileStorageService.UpdateAsync(fileStream, imageFile.FileName, 
            imageFile.ContentType, updatedEventModel.Id);
        
        updatedEventModel.ImageUrl = imageUrl;
        return updatedEventModel;
    }

    public async Task<EventModel?> DeleteAsync(Guid eventId)
    {
        var deletedEventModel = await _eventRepository.DeleteByIdAsync(eventId);

        if (deletedEventModel is null) {
            return null;
        }
        
        // удаляем изображение события
        await _fileStorageService.DeleteAsync(deletedEventModel.ImageFile);
        return deletedEventModel;
    }

    public async Task<PaginatedList<EventModel>> GetByFilter(DateTime? date, string? location, 
        string? category, int pageIndex, int pageSize)
    {
        var events = await _eventRepository
            .GetByFilterAsync(date, location, category, pageIndex, pageSize);

        if (events.Items.Count == 0) {
            return events;
        }
        
        foreach (var eventModel in events.Items)
        {
            eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.ImageFile);
        }
        return events;
    }

    public async Task<Result<string>> AddImageAsync(Guid eventId, IFormFile imageFile)
    {
        var eventModel = await _eventRepository.GetByIdAsync(eventId);

        if (eventModel is null)
        {
            _logger.LogWarning("Событие {eventId} не найдено", eventId);
            return Result.Fail(new EventWithIdNotFoundError(eventId));
        }
        if (eventModel?.ImageFile is not null)
        {
            _logger.LogWarning("Событие {eventId} уже имеет изображение", eventId);
            return Result.Fail(new Error($"Событие {eventId} уже имеет изображение"));
        }
        
        await using var stream = imageFile.OpenReadStream();
        var imageUrl = await _fileStorageService.UploadAsync(stream, imageFile.FileName, 
            imageFile.ContentType, eventId);
        
        _logger.LogInformation("Изображение успешно добавлено к событию {eventId}", eventId);
        return imageUrl;
    }
    
    public async Task<Result<bool>> RegisterToEventAsync(Guid eventId, Guid userId)
    {
        var isRegistered = await ValidateEventAndUserAsync(eventId, userId);
        
        if (isRegistered.IsFailed)
        {
            return Result.Fail(isRegistered.Errors.First());
        }
        
        // пользователь уже подписан на это событие
        if (isRegistered.Value != null) return false;

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

    public async Task<List<UserModel>?> GetParticipantsByIdAsync(Guid eventId)
    {
        var eventModel = await _eventRepository.GetByIdAsync(eventId);
        if (eventModel is null) return null;

        var participants = eventModel.EventUsers
            .Select(eventUser => eventUser.User)
            .ToList();
        return participants;
    }

    public async Task<Result<UserModel>> GetParticipantByIdAsync(Guid eventId, Guid userId)
    {
        var eventUserResult = await ValidateEventAndUserAsync(eventId, userId);
        if (eventUserResult.IsFailed) 
        {
            return Result.Fail(eventUserResult.Errors.First());
        }
        
        if (eventUserResult.Value == null)
            return Result.Fail(new Error($"Пользователь {userId} не был зарегистрирован на событие {eventId}"));

        return eventUserResult.Value.User;
    }

    public async Task<Result<bool>> CancelEventParticipationAsync(Guid eventId, Guid userId)
    {
        var eventUserResult = await ValidateEventAndUserAsync(eventId, userId);
        
        if (eventUserResult.IsFailed) 
        {
            return Result.Fail(eventUserResult.Errors.First());
        }
        
        if (eventUserResult.Value == null)
            return Result.Fail(new Error($"Пользователь {userId} не был зарегистрирован на событие {eventId}"));

        // удаляем подписку пользователя на событие
        _ = await _eventUserRepository.DeleteByEventAndUserIdAsync(eventId, userId);
        return true;
    }
    
    private async Task<Result<EventUserModel?>> ValidateEventAndUserAsync(Guid eventId, Guid userId)
    {
        var eventModel = await _eventRepository.GetByIdAsync(eventId);
        if (eventModel is null)
            return Result.Fail(new EventWithIdNotFoundError(eventId));
        
        var userModel = await _userRepository.GetByIdAsync(userId);
        if (userModel == null)
            return Result.Fail(new UserWithIdNotFoundError(userId));
        
        var registeredEventUser = await _eventUserRepository.GetByEventAndUserIdAsync(eventId, userId);
        return registeredEventUser;
    }
}
