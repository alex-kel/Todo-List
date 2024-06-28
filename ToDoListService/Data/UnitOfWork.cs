using ToDoListService.Data.Interfaces;
using ToDoListService.Repositories.Interfaces;

namespace ToDoListService.Data;

public sealed class UnitOfWork(TodoContext context, ITodoItemRepository todoItems) : IUnitOfWork, IDisposable
{

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public void SaveChanges()
    { 
        context.SaveChanges();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}