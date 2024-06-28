namespace ToDoListService.Models;

public class BaseEntity
{
    public long Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}