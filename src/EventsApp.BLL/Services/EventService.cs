using EventsApp.DAL.Entities;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Abstractions.Files;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace EventsApp.BLL.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<EventService> _logger;
    
    public EventService(IEventRepository eventRepository, IFileStorageService fileService, ILogger<EventService> logger)
    {
        _eventRepository = eventRepository;
        _fileStorageService = fileService;
        _logger = logger;
    }

    public async Task<List<EventModel>> GetAllAsync()
    {
        var eventModels = await _eventRepository.GetAllAsync();

        if (eventModels.Count == 0)
        {
            return [];
        }

        foreach (var eventModel in eventModels)
        {
            eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.Id);
        }
        return eventModels;
    }
    
    public async Task<EventModel?> GetByIdAsync(Guid eventId)
    {
        var eventModel = await _eventRepository.GetByIdAsync(eventId);

        if (eventModel is null)
        {
            return null;
        }
        
        eventModel.ImageUrl = await _fileStorageService.GetPreSignedUrl(eventModel.Id);
        return eventModel;
    }

    public async Task<EventModel?> GetByNameAsync(string name)
    {
        return await _eventRepository.GetByNameAsync(name);
    }

    /// <summary>
    /// Добавление события с его изображением
    /// </summary>
    /// <param name="eventModel"></param>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="mimeType"></param>
    /// <returns>Добавленное событие и его изображение</returns>
    public async Task<Result<EventModel>> AddAsync(EventModel eventModel, Stream fileStream, string fileName, string mimeType)
    {
        try
        {
            var eventId = Guid.NewGuid();
            eventModel.Id = eventId;
            
            var result = await _eventRepository.AddAsync(eventModel);
            
            // сохранение картинки события
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
}

