using ToDoListService.Models;

namespace ToDoListService.Repositories.Interfaces;

public interface ITodoItemRepository : ICrudRepository<TodoItem>
{
    // Methods definitions which are specific only for TodoItem entities...
}