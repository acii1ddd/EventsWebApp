namespace EventsApp.Domain.Abstractions;

public interface IRepository<T>
{
    public Task<List<T>> GetAllAsync();
    
    public Task<T?> GetByIdAsync(Guid id);
    
    public Task<T> AddAsync(T entity);

    public Task<T?> UpdateAsync(T newEntity);

    public Task<T?> DeleteByIdAsync(Guid id);
    
    
}
