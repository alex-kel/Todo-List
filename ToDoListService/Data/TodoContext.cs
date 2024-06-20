using Microsoft.EntityFrameworkCore;
using ToDoListService.Models;

namespace ToDoListService.Data;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    private const string TodoItemsIdSequenceName = "TodoItemIdSequence";
    
    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ConfigureTodoItemHiLo(modelBuilder);
    }

    private void ConfigureTodoItemHiLo(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<long>(TodoItemsIdSequenceName)
            .IncrementsBy(100);

        modelBuilder.Entity<TodoItem>()
            .Property(item => item.Id)
            .UseHiLo(TodoItemsIdSequenceName);
    }
}