using ToDoListService.DTOs;
using ToDoListService.Mappers;
using ToDoListService.Models;

namespace ToDoListServiceTests.Mappers;

public class TodoItemMapperTest
{
    [Fact]
    public void MapsToDtoCorrectly()
    {
        var todoItemEntity = new TodoItem()
        {
            Id = 1,
            IsComplete = true,
            Name = "Some Todo Item"
        };
        var todoItemDto = TodoItemMapper.ToTodoItemDto(todoItemEntity);
        Assert.Equal(todoItemEntity.Id, todoItemDto.Id);
        Assert.Equal(todoItemEntity.IsComplete, todoItemDto.IsComplete);
        Assert.Equal(todoItemEntity.Name, todoItemDto.Name);
    }
    
    [Fact]
    public void MapsToEntityCorrectly()
    {
        var todoItemDto = new TodoItemDto()
        {
            Id = 1,
            IsComplete = true,
            Name = "Some Todo Item"
        };
        var todoItemEntity = TodoItemMapper.ToTodoItem(todoItemDto);
        Assert.Equal(todoItemDto.Id, todoItemEntity.Id);
        Assert.Equal(todoItemDto.IsComplete, todoItemEntity.IsComplete);
        Assert.Equal(todoItemDto.Name, todoItemEntity.Name);
        Assert.Null(todoItemEntity.CreatedAt);
        Assert.Null(todoItemEntity.UpdateAt);
    }
}