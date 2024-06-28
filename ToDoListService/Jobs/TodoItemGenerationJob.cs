using MassTransit;
using ToDoListService.Contracts;
using ToDoListService.Jobs.Interfaces;

namespace ToDoListService.Jobs;

public class TodoItemGenerationJob(ILogger<TodoItemGenerationJob> logger, IBus bus) : ITodoItemsGenerationJob
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Generation was triggered");
        await bus.Publish(new TodoItemCreateMessage { Name = $"Generated at {DateTimeOffset.Now}" }, cancellationToken);
        logger.LogInformation("Message has been published to RabbitMQ");
    }
}