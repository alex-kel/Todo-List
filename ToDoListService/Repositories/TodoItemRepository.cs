using ToDoListService.Data;
using ToDoListService.Models;
using ToDoListService.Repositories.Interfaces;

namespace ToDoListService.Repositories;

public class TodoItemRepository(TodoContext context, ILogger<TodoItemRepository> logger) : GenericCrudRepository<TodoItem>(context, logger), ITodoItemRepository
{
    public override async Task<bool> UpdateAsync(TodoItem entity)
    {
        var todoItem = await GetAsync(entity.Id);
        if (todoItem == null)
        {
            logger.LogWarning("TodoItems with {Id} is not found", entity.Id);
            return false;
        }

        todoItem.Name = entity.Name;
        todoItem.IsComplete = entity.IsComplete;

        return true;
    }
}