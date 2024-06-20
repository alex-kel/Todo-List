using System.ComponentModel.DataAnnotations;

namespace ToDoListService.Models;

public class TodoItem : BaseEntity
{
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}