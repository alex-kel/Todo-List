namespace ToDoListService.Contracts;

public record TodoItemCreateMessage
{
    public required string Name { get; init; }
}