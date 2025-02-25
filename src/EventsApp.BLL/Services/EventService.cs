using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Abstractions.Files;
using EventsApp.Domain.Errors;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EventsApp.BLL.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<EventService> _logger;
    
    public EventService(IEventRepository eventRepository, IFileStorageService fileService, 
        ILogger<EventService> logger)
    {
        _eventRepository = eventRepository;
        _fileStorageService = fileService;
        _logger = logger;
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
}
