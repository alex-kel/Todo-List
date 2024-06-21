namespace ToDoListService.DTOs;

public class CommonErrorResponseDto<T> where T : class
{
    public required string Status { get; set; }
    public required int StatusCode { get; set; }
    public required T? FailureDetails { get; set; }
}