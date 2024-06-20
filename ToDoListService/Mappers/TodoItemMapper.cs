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

    public static TodoItem ToTodoItem(TodoItemDto todoItemDto)
    {
        return new TodoItem()
        {
            Id = todoItemDto.Id,
            Name = todoItemDto.Name,
            IsComplete = todoItemDto.IsComplete
        };
    }
}