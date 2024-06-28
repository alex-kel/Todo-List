using Microsoft.EntityFrameworkCore;
using ToDoListService.Data;
using ToDoListService.Repositories.Interfaces;

namespace ToDoListService.Repositories;

public abstract class GenericCrudRepository<T>(TodoContext context, ILogger<GenericCrudRepository<T>> logger)
    : ICrudRepository<T> where T : class
{
    protected readonly DbSet<T> DbSet = context.Set<T>();
    
    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetAsync(long id, CancellationToken cancellationToken)
    {
        try
        {
            return await DbSet.FindAsync(new object?[] { id }, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting entity with id {Id}", id);
            return null;
        }
    }

    public virtual async Task<bool> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        try
        {
            await DbSet.AddAsync(entity, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to create an entity");
            return false;
        }
    }
    
    public virtual bool Create(T entity)
    {
        try
        {
            DbSet.Add(entity);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to create an entity");
            return false;
        }
    }

    // This needs to be implemented in Entity specific repository.
    public abstract Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken);

    public virtual async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await DbSet.FindAsync(new object?[] { id }, cancellationToken);
        if (entity != null)
        {
            DbSet.Remove(entity);
            return true;
        }

        logger.LogWarning("Entity with id {Id} not found for deletion", id);
        return false;
    }
}
