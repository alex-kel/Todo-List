using ToDoListService.DTOs;
using ToDoListService.Models;

namespace ToDoListService.Mappers;

public static class TodoItemMapper
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