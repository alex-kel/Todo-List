using ToDoListService.Dtos;
using ToDoListService.Models;

namespace ToDoListService.Mappers;

public class TodoItemMapper
{
    public static TodoItemDto ToTodoItemDto(TodoItem todoItem)
    {
        return new TodoItemDto()
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
    }
}