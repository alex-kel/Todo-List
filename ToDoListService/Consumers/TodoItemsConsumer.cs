using MassTransit;
using ToDoListService.Contracts;
using ToDoListService.Data.Interfaces;
using ToDoListService.Models;
using ToDoListService.Repositories.Interfaces;

namespace ToDoListService.Consumers;

// ReSharper disable once ClassNeverInstantiated.Global
public class TodoItemsConsumer(
    ILogger<TodoItemsConsumer> logger,
    IUnitOfWork unitOfWork,
    ITodoItemRepository todoItemRepository) : IConsumer<TodoItemCreateMessage>
{
    public Task Consume(ConsumeContext<TodoItemCreateMessage> context)
    {
        logger.LogInformation("Received message for item creation with Name: {Name}", context.Message.Name);
        var todoItem = new TodoItem
        {
            Name = context.Message.Name,
            IsComplete = false
        };
        todoItemRepository.Create(todoItem);
        unitOfWork.SaveChanges();
        return Task.CompletedTask;
    }
}