using ToDoListService.Data;
using ToDoListService.Repositories.Interfaces;

namespace ToDoListService.Repositories;

public sealed class UnitOfWork(TodoContext context, ITodoItemRepository todoItems) : IUnitOfWork, IDisposable
{
    public ITodoItemRepository TodoItems { get; } = todoItems;

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}