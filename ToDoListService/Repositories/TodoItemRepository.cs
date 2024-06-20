using ToDoListService.Data;
using ToDoListService.Models;
using ToDoListService.Repositories.Interfaces;

namespace ToDoListService.Repositories;

public class TodoItemRepository(TodoContext context, ILogger<TodoItemRepository> logger)
    : GenericBaseEntityCrudRepository<TodoItem>(context, logger), ITodoItemRepository;