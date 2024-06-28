namespace ToDoListService.Jobs.Interfaces;

public interface IGenericJob
{
    public Task RunAsync(CancellationToken cancellationToken);
}