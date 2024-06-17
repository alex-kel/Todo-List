using Microsoft.EntityFrameworkCore;
using ToDoListService.DTOs;
using ToDoListService.Mappers;
using ToDoListService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("TodoContext")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

var todoItems = app.MapGroup("api/todoitems");
todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/{id}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);
todoItems.MapDelete("/{id}", DeleteTodo);

app.Run();

static async Task<IResult> GetAllTodos(TodoContext db)
{
    return TypedResults.Ok(await db.TodoItems.Select(x => TodoItemMapper.ToTodoItemDto(x)).ToArrayAsync());
}

static async Task<IResult> GetTodo(int id, TodoContext db)
{
    return await db.TodoItems.FindAsync(id)
        is { } todo
        ? TypedResults.Ok(TodoItemMapper.ToTodoItemDto(todo))
        : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(TodoItemDto todoDto, TodoContext db)
{
    var todoItem = new TodoItem
    {
        IsComplete = todoDto.IsComplete,
        Name = todoDto.Name
    };
    db.TodoItems.Add(todoItem);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/api/todoitems/{todoItem.Id}", todoItem);
}

static async Task<IResult> UpdateTodo(long id, TodoItemDto inputTodo, TodoContext db)
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo is null) return TypedResults.NotFound();
    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(long id, TodoContext db)
{
    if (await db.TodoItems.FindAsync(id) is not { } todo) return TypedResults.NotFound();
    db.TodoItems.Remove(todo);
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}