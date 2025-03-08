using AutoMapper;
using EventsApp.BLL.Interfaces;
using EventsApp.DAL.Entities;
using EventsApp.DAL.Interfaces;
using EventsApp.Domain.Exceptions;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;
using EventsApp.Domain.Models.EventUsers;
using EventsApp.Domain.Models.Participants;
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
    private readonly IMapper _mapper;
    
    public EventService(
        IEventRepository eventRepository, 
        IFileStorageService fileService, 
        ILogger<EventService> logger, 
        IUserRepository userRepository, 
        IEventUserRepository eventUserRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _fileStorageService = fileService;
        _logger = logger;
        _userRepository = userRepository;
        _eventUserRepository = eventUserRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<EventModel>> GetAllAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var paginatedEventEntities = await _eventRepository.GetAllAsync(pageIndex, pageSize, cancellationToken);
        var paginatedEvents = _mapper.Map<PaginatedList<EventModel>>(paginatedEventEntities);
        
        if (paginatedEvents.Items.Count == 0) {
            return paginatedEvents;
        }
    
        foreach (var eventModel in paginatedEvents.Items)
        {
            eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.ImageFile);
        }
        return paginatedEvents;
    }
    
    public async Task<EventModel> GetByIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var eventModel = _mapper.Map<EventModel>(await _eventRepository.GetByIdAsync(eventId, cancellationToken));
        
        if (eventModel is null)
        {
            throw new NotFoundException($"Событие с Id {eventId} не найдено");
        }

        eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.ImageFile);
        return eventModel;
    }

    public async Task<EventModel> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var eventModel = _mapper.Map<EventModel>(await _eventRepository.GetByNameAsync(name, cancellationToken));
        
        if (eventModel is null)
        {
            throw new NotFoundException($"Событие с {name} не найдено");
        }
        
        eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.ImageFile);
        return eventModel;
    }

    /// <summary>
    /// Добавление события с его изображением
    /// </summary>
    /// <param name="eventModel"></param>
    /// <param name="imageFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Добавленное событие и его изображение</returns>
    public async Task<EventModel> AddAsync(EventModel eventModel, IFormFile? imageFile, CancellationToken cancellationToken)
    {
        var eventId = Guid.NewGuid();
        eventModel.Id = eventId;
        eventModel.StartDate = eventModel.StartDate.ToUniversalTime();
        
        var result = _mapper.Map<EventModel>(
            await _eventRepository.AddAsync(_mapper.Map<EventEntity>(eventModel), cancellationToken)
        );
        
        // у события нет изображения
        if (imageFile is null) {
            return result;
        }
        
        // сохранение изображение события
        await using var fileStream = imageFile.OpenReadStream();

        result.ImageUrl = await _fileStorageService
            .UploadAsync(fileStream, imageFile.FileName, imageFile.ContentType, eventId, cancellationToken);
        
        _logger.LogInformation("Событие {eventName} успешно добавлено", eventModel.Name);
        return result;
    }
    
    public async Task<EventModel> UpdateAsync(EventModel eventModel, IFormFile imageFile, CancellationToken cancellationToken)
    {
        if (await _eventRepository.GetByIdAsync(eventModel.Id, cancellationToken) is null)
        {
            throw new NotFoundException($"Событие с Id {eventModel.Id} не найдено");
        }
        
        // сохраняем дату в utc
        eventModel.StartDate = eventModel.StartDate.ToUniversalTime();
        var updatedEventModel = _mapper.Map<EventModel>(
            await _eventRepository.UpdateAsync(_mapper.Map<EventEntity>(eventModel), cancellationToken)
        );

        // поток для обновления изображения в minio
        await using var fileStream = imageFile.OpenReadStream();
        
        // обновляем изображение события
        var imageUrl = await _fileStorageService.UpdateAsync(fileStream, imageFile.FileName, 
            imageFile.ContentType, updatedEventModel.Id, cancellationToken);
        
        updatedEventModel.ImageUrl = imageUrl;
        return updatedEventModel;
    }

    public async Task<EventModel> DeleteAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(eventId, cancellationToken);
        if (eventEntity is null)
        {
            throw new NotFoundException($"Событие с Id {eventId} не найдено");
        }
        
        var deletedEventModel = _mapper.Map<EventModel>(await _eventRepository.DeleteAsync(eventEntity, cancellationToken));

        // удаляем изображение события
        await _fileStorageService.DeleteAsync(deletedEventModel.ImageFile, cancellationToken);
        return deletedEventModel;
    }

    public async Task<PaginatedList<EventModel>> GetByFilter(DateTime? date, string? location, 
        string? category, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var paginatedEventEntities = await _eventRepository
            .GetByFilterAsync(date, location, category, pageIndex, pageSize, cancellationToken);
        
        var paginatedEvents = _mapper.Map<PaginatedList<EventModel>>(paginatedEventEntities);

        if (paginatedEvents.Items.Count == 0) {
            return paginatedEvents;
        }
        
        foreach (var eventModel in paginatedEvents.Items)
        {
            eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.ImageFile);
        }
        return paginatedEvents;
    }

    public async Task<string> AddImageAsync(Guid eventId, IFormFile imageFile, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(eventId, cancellationToken);

        if (eventEntity is null)
        {
            _logger.LogWarning("Событие {eventId} не найдено", eventId);
            throw new NotFoundException($"Событие с Id {eventId} не найдено");
        }
        if (eventEntity.ImageFile is not null)
        {
            _logger.LogWarning("Событие {eventId} уже имеет изображение", eventId);
            throw new InvalidOperationException($"Событие {eventId} уже имеет изображение");
        }
        
        await using var stream = imageFile.OpenReadStream();
        var imageUrl = await _fileStorageService.UploadAsync(stream, imageFile.FileName, 
            imageFile.ContentType, eventId, cancellationToken);
        
        _logger.LogInformation("Изображение успешно добавлено к событию {eventId}", eventId);
        return imageUrl;
    }
    
    public async Task RegisterToEventAsync(Guid eventId, Guid userId, CancellationToken cancellationToken)
    {
        // пользователь уже подписан на это событие
        if (await GetEventUserAsync(eventId, userId, cancellationToken) != null)
        {
            throw new InvalidOperationException($"Пользователь {userId} уже зарегистрирован на событие {eventId}");
        }

        // подписываем пользователя на событие
        var eventUserModel = new EventUserModel
        {
            EventId = eventId,
            UserId = userId,
            RegisteredAt = DateTime.UtcNow
        };
        _ = await _eventUserRepository.AddAsync(_mapper.Map<EventUserEntity>(eventUserModel), cancellationToken);
    }

    public async Task<List<UserModel>> GetParticipantsByIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var eventModel = _mapper.Map<EventModel>(await _eventRepository.GetByIdAsync(eventId, cancellationToken));
        if (eventModel is null)
        {
            throw new NotFoundException($"Событие с Id {eventId} не найдено");
        }

        var participants = eventModel.EventUsers
            .Select(eventUser => eventUser.User)
            .ToList();
        return participants;
    }

    public async Task<UserModel> GetParticipantByIdAsync(Guid eventId, Guid userId, CancellationToken cancellationToken)
    {
        var eventUser = await GetEventUserAsync(eventId, userId, cancellationToken);
        if (eventUser is null)
        {
            throw new InvalidOperationException($"Пользователь {userId} не был зарегистрирован на событие {eventId}");
        }

        return eventUser.User;
    }

    public async Task CancelEventParticipationAsync(Guid eventId, Guid userId, CancellationToken cancellationToken)
    {
        var eventUser = await GetEventUserAsync(eventId, userId, cancellationToken);
        
        if (eventUser is null)
        {
            throw new InvalidOperationException($"Пользователь {userId} не был зарегистрирован на событие {eventId}");
        }
        
        // удаляем подписку пользователя на событие
        _ = await _eventUserRepository.DeleteEventUserAsync(_mapper.Map<EventUserEntity>(eventUser), cancellationToken);
    }
    
    private async Task<EventUserModel?> GetEventUserAsync(Guid eventId, Guid userId, CancellationToken cancellationToken)
    {
        if (await _eventRepository.GetByIdAsync(eventId, cancellationToken) is null)
        {
            throw new NotFoundException($"Событие с Id {eventId} не найдено");
        }
        
        if (await _userRepository.GetByIdAsync(userId, cancellationToken) is null) 
        {
            throw new NotFoundException($"Пользователь с Id {eventId} не найдено");
        }
        
        var registeredEventUser = _mapper.Map<EventUserModel>(
            await _eventUserRepository.GetByEventAndUserIdAsync(eventId, userId, cancellationToken)
        );
        return registeredEventUser;
    }
}
