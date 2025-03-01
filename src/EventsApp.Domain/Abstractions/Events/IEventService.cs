using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;
using EventsApp.Domain.Models.Participants;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace EventsApp.Domain.Abstractions.Events;

public interface IEventService
{
    public Task<PaginatedList<EventModel>> GetAllAsync(int pageIndex, int pageSize);

    public Task<EventModel?> GetByIdAsync(Guid eventId);
    
    public Task<EventModel?> GetByNameAsync(string name);

    public Task<Result<EventModel>> AddAsync(EventModel eventModel, IFormFile? imageFile);

    /// <summary>
    /// Обновление события
    /// </summary>
    /// <param name="eventModel">Новое событие</param>
    /// <param name="file"></param>
    /// <returns>Обновленное событие / null, если событие на было обновлено</returns>
    public Task<EventModel?> UpdateAsync(EventModel eventModel, IFormFile file);

    public Task<EventModel?> DeleteAsync(Guid eventId);

    public Task<PaginatedList<EventModel>> GetByFilter(DateTime? date, string? location,
        string? category, int pageIndex, int pageSize);

    public Task<Result<string>> AddImageAsync(Guid eventId, IFormFile imageFile);

    public Task<Result<bool>> RegisterToEventAsync(Guid eventId, Guid userId);
    
    public Task<List<UserModel>?> GetParticipantsByIdAsync(Guid eventId);

    /// <summary>
    /// Получение пользователя события eventId по Id userId 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<Result<UserModel>> GetParticipantByIdAsync(Guid eventId, Guid userId);

    public Task<Result<bool>> CancelEventParticipationAsync(Guid eventId, Guid authorizedUserId);
}
