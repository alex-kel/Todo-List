using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListService.Models;
using ToDoListService.Dtos;
using ToDoListService.Mappers;

namespace ToDoListService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController(TodoContext context) : ControllerBase
    {
        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
        {
            return await context.TodoItems
                .Select(x => TodoItemMapper.ToTodoItemDto(x))
                .ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id:long}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItem(long id)
        {
            var todoItem = await context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return TodoItemMapper.ToTodoItemDto(todoItem);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id:long}")]
        public async Task<ActionResult<TodoItemDto>> PutTodoItem(long id, TodoItemDto todoItemDto)
        {
            if (id != todoItemDto.Id)
            {
                return BadRequest();
            }
            
            var todoItem = await context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoItemDto.Name;
            todoItem.IsComplete = todoItemDto.IsComplete;
            
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id },
                TodoItemMapper.ToTodoItemDto(todoItem));
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> PostTodoItem(TodoItemDto todoItemDto)
        {
            var todoItem = new TodoItem
            {
                Name = todoItemDto.Name,
                IsComplete = todoItemDto.IsComplete
            };
            context.TodoItems.Add(todoItem);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id },
                TodoItemMapper.ToTodoItemDto(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            context.TodoItems.Remove(todoItem);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return context.TodoItems.Any(e => e.Id == id);
        }
    }
}
