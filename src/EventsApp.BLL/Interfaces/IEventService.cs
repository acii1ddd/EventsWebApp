using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;
using EventsApp.Domain.Models.Participants;
using Microsoft.AspNetCore.Http;

namespace EventsApp.BLL.Interfaces;

public interface IEventService
{
    public Task<PaginatedList<EventModel>> GetAllAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);

    public Task<EventModel> GetByIdAsync(Guid eventId, CancellationToken cancellationToken);
    
    public Task<EventModel> GetByNameAsync(string name, CancellationToken cancellationToken);

    public Task<EventModel> AddAsync(EventModel eventModel, IFormFile? imageFile, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление события
    /// </summary>
    /// <param name="eventModel">Новое событие</param>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Обновленное событие / null, если событие на было обновлено</returns>
    public Task<EventModel> UpdateAsync(EventModel eventModel, IFormFile file, CancellationToken cancellationToken);

    public Task<EventModel> DeleteAsync(Guid eventId, CancellationToken cancellationToken);

    public Task<PaginatedList<EventModel>> GetByFilter(DateTime? date, string? location,
        string? category, int pageIndex, int pageSize, CancellationToken cancellationToken);

    public Task<string> AddImageAsync(Guid eventId, IFormFile imageFile, CancellationToken cancellationToken);

    public Task RegisterToEventAsync(Guid eventId, Guid userId, CancellationToken cancellationToken);
    
    public Task<List<UserModel>> GetParticipantsByIdAsync(Guid eventId, CancellationToken cancellationToken);

    /// <summary>
    /// Получение пользователя события eventId по Id userId 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    public Task<UserModel> GetParticipantByIdAsync(Guid eventId, Guid userId, CancellationToken cancellationToken);

    public Task CancelEventParticipationAsync(Guid eventId, Guid userId, CancellationToken cancellationToken);
}
