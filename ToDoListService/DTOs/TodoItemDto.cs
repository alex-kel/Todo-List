using System.ComponentModel.DataAnnotations;
using ToDoListService.Validators;

namespace ToDoListService.DTOs;

public class TodoItemDto
{

    [MatchesRouteValue("TodoItems","PutTodoItemAsync", "id")]
    public long Id { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(140, ErrorMessage = "Todo Item name length can't be more than 140.")]
    public required string Name { get; set; }
    
    public bool IsComplete { get; set; }
    
    private bool Equals(TodoItemDto other)
    {
        return Id == other.Id && Name == other.Name && IsComplete == other.IsComplete;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TodoItemDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, IsComplete);
    }
}