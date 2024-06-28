namespace ToDoListService.Repositories.Interfaces;

public interface ICrudRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    
    public Task<T?> GetAsync(long id, CancellationToken cancellationToken);
    
    public Task<bool> CreateAsync(T entity, CancellationToken cancellationToken);

    public bool Create(T entity);
    
    public Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken);
    
    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken); 
}