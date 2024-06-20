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
}