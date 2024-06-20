using Microsoft.EntityFrameworkCore;
using ToDoListService.Models;

namespace ToDoListService.Data;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; }
}