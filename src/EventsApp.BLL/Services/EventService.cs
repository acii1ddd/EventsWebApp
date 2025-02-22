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
        return await _eventRepository.GetAllAsync();
    }
    
    public async Task<EventModel?> GetByIdAsync(Guid eventId)
    {
        return await _eventRepository.GetByIdAsync(eventId);
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
    /// <param name="userId"></param>
    /// <returns>Добавленное изображение</returns>
    public async Task<Result<EventModel>> AddAsync(EventModel eventModel, Stream fileStream, string fileName, string mimeType, Guid userId)
    {
        try
        {
            var eventId = Guid.NewGuid();
            
            var imageFileModel = await _fileStorageService.UploadAsync(fileStream, fileName, mimeType, eventId);
            
            eventModel.Id = eventId;
            var result = await _eventRepository.AddAsync(eventModel);
        
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

