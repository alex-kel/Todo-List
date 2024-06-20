namespace ToDoListService.Repositories.Interfaces;

public interface ICrudRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAllAsync();
    
    public Task<T?> GetAsync(long id);
    
    public Task<bool> CreateAsync(T entity);
    
    public Task<bool> UpdateAsync(T entity);
    
    public Task<bool> DeleteAsync(long id); 
}