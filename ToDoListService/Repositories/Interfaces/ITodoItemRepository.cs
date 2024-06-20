using ToDoListService.Models;

namespace ToDoListService.Repositories.Interfaces;

public interface ITodoItemRepository : ICrudRepository<TodoItem>
{
    // Methods specific for TodoItem models...
}