using Microsoft.EntityFrameworkCore;
using ToDoListService.Data;
using ToDoListService.Repositories.Interfaces;

namespace ToDoListService.Repositories;

public abstract class GenericCrudRepository<T>(TodoContext context, ILogger<GenericCrudRepository<T>> logger) : ICrudRepository<T> where T : class
{
    protected DbSet<T> DbSet = context.Set<T>();
    
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<T?> GetAsync(long id)
    {
        try
        {
            return await DbSet.FindAsync(id);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting entity with id {Id}", id);
            return null;
        }
    }

    public virtual async Task<bool> CreateAsync(T entity)
    {
        try
        {
            await DbSet.AddAsync(entity);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to create an entity");
            return false;
        }
    }

    // This needs to be implemented in Entity specific repository.
    public abstract Task<bool> UpdateAsync(T entity);

    public virtual async Task<bool> DeleteAsync(long id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
            return true;
        }

        logger.LogWarning("Entity with id {Id} not found for deletion", id);
        return false;
    }
}