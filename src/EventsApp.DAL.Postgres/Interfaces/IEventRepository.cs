using EventsApp.DAL.Entities;
using EventsApp.Domain.Models;
using EventsApp.Domain.Models.Events;

namespace EventsApp.DAL.Interfaces;

public interface IEventRepository
{
    public Task<PaginatedList<EventEntity>> GetAllAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
    
    public Task<EventEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    public Task<EventEntity?> GetByNameAsync(string name, CancellationToken cancellationToken);
    
    public Task<EventEntity> AddAsync(EventEntity entity, CancellationToken cancellationToken);

    public Task<EventEntity> UpdateAsync(EventEntity newEntity, CancellationToken cancellationToken);

    public Task<EventEntity> DeleteAsync(EventEntity entity, CancellationToken cancellationToken);
    
    public Task<PaginatedList<EventEntity>> GetByFilterAsync(DateTime? minDate, string? location, 
        string? category, int pageIndex, int pageSize, CancellationToken cancellationToken);
}
