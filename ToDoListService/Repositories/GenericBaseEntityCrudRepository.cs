using ToDoListService.Data;
using ToDoListService.Models;

namespace ToDoListService.Repositories;

public abstract class GenericBaseEntityCrudRepository<T>(TodoContext context, ILogger<GenericBaseEntityCrudRepository<T>> logger) 
    : GenericCrudRepository<T>(context, logger) where T : BaseEntity
{
    private readonly TodoContext _context = context;

    public override async Task<bool> CreateAsync(T entity, CancellationToken cancellationToken)
    {
        entity.CreatedAt = DateTime.Now;
        return await base.CreateAsync(entity, cancellationToken);
    }

    public override async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var persistedEntity = await GetAsync(entity.Id, cancellationToken);

        if (persistedEntity == null)
        {
            logger.LogWarning("Entity with {Id} is not found", entity.Id);
            return false;
        }

        // CreatedAt should not be modifiable
        entity.CreatedAt = persistedEntity.CreatedAt;
        entity.UpdateAt = DateTime.Now;

        _context.Entry(persistedEntity).CurrentValues.SetValues(entity);

        return true;
    }
}
