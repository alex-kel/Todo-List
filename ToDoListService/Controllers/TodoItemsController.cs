using Microsoft.AspNetCore.Mvc;
using ToDoListService.Data.Interfaces;
using ToDoListService.Models;
using ToDoListService.DTOs;
using ToDoListService.Mappers;
using ToDoListService.Repositories.Interfaces;

namespace ToDoListService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController(
    IUnitOfWork unitOfWork,
    ITodoItemRepository todoItemRepository
) : ControllerBase
{
    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItemsAsync(CancellationToken cancellationToken)
    {
        var todoItems = await todoItemRepository.GetAllAsync(cancellationToken);
        return Ok(todoItems.Select(TodoItemMapper.ToTodoItemDto));
    }

    // GET: api/TodoItems/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<TodoItemDto>> GetTodoItemAsync(long id, CancellationToken cancellationToken)
    {
        var todoItem = await todoItemRepository.GetAsync(id, cancellationToken);

        if (todoItem == null) return NotFound();

        return TodoItemMapper.ToTodoItemDto(todoItem);
    }

    // POST: api/TodoItems
    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> PostTodoItem(TodoItemDto todoItemDto, CancellationToken cancellationToken)
    {
        var todoItem = new TodoItem
        {
            Name = todoItemDto.Name,
            IsComplete = todoItemDto.IsComplete
        };
        await todoItemRepository.CreateAsync(todoItem, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetTodoItemAsync), new { id = todoItem.Id },
            TodoItemMapper.ToTodoItemDto(todoItem));
    }
        
    // PUT: api/TodoItems/5
    [HttpPut("{id:long}")]
    public async Task<ActionResult<TodoItemDto>> PutTodoItemAsync(long id, TodoItemDto todoItemDto, CancellationToken cancellationToken)
    {
        var todoItem = TodoItemMapper.ToTodoItem(todoItemDto);
        var isUpdated = await todoItemRepository.UpdateAsync(todoItem, cancellationToken);
        if (!isUpdated) return BadRequest();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetTodoItemAsync), new { id = todoItem.Id },
            TodoItemMapper.ToTodoItemDto(todoItem));
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteTodoItem(long id, CancellationToken cancellationToken)
    {
        var todoItem = await todoItemRepository.GetAsync(id, cancellationToken);
        if (todoItem == null)
        {
            return NotFound();
        }

        await todoItemRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
