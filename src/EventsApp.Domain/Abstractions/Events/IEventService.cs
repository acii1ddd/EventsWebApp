using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;
using FluentResults;

namespace EventsApp.Domain.Abstractions.Events;

public interface IEventService
{
    public Task<List<EventModel>> GetAllAsync();

    public Task<EventModel?> GetByIdAsync(Guid eventId);
    
    public Task<EventModel?> GetByNameAsync(string name);

    public Task<Result<EventModel>> AddAsync(EventModel eventModel, Stream? fileStream, string fileName, string mimeType);

    /// <summary>
    /// Обновление события
    /// </summary>
    /// <param name="eventModel">Новое событие</param>
    /// <param name="fileStream">Поток для обновления изображения</param>
    /// <param name="fileName">Новое название изображения</param>
    /// <param name="mimeType">Новый mime type изображения</param>
    /// <returns>Обновленное событие / null, если событие на было обновлено</returns>
    public Task<EventModel?> UpdateAsync(EventModel eventModel, Stream fileStream, string fileName, 
        string mimeType);

    public Task<EventModel?> DeleteAsync(Guid eventId);
}
