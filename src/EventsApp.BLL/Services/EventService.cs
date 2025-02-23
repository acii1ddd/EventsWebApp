using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Abstractions.Files;
using EventsApp.Domain.Models.Events;
using FluentResults;
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

    public async Task<List<EventModel>> GetAllAsync()
    {
        var eventModels = await _eventRepository.GetAllAsync();
    
        if (eventModels.Count == 0) {
            return [];
        }
    
        foreach (var eventModel in eventModels)
        {
            eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.ImageFile);
        }
        return eventModels;
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
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="mimeType"></param>
    /// <returns>Добавленное событие и его изображение</returns>
    public async Task<Result<EventModel>> AddAsync(EventModel eventModel, Stream? fileStream, 
        string fileName, string mimeType)
    {
        try
        {
            var eventId = Guid.NewGuid();
            eventModel.Id = eventId;
            
            var result = await _eventRepository.AddAsync(eventModel);
            
            // у события нет изображения
            if (fileStream is null) {
                return result;
            }
            
            // сохранение изображение события
            var imageUrl = await _fileStorageService.UploadAsync(fileStream, fileName, mimeType, eventId);
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
        Stream fileStream, string fileName, string mimeType)
    {
        var updatedEventModel = await _eventRepository.UpdateAsync(eventModel);

        if (updatedEventModel is null) {
            return null;
        }

        // обновляем изображение события
        var imageUrl = await _fileStorageService.UpdateAsync(fileStream, fileName, 
            mimeType, updatedEventModel.Id);
        
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
}

