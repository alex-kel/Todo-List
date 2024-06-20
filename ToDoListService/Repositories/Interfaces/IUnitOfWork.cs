using Microsoft.EntityFrameworkCore;

namespace ToDoListService.Repositories.Interfaces;

public interface IUnitOfWork
{
    ITodoItemRepository TodoItems { get; }

    public Task SaveChangesAsync();
}