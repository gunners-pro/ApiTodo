using ApiTodo.App.Context;
using ApiTodo.App.DTOs.Todo;
using ApiTodo.App.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiTodo.App.Repositories.Todos;

public class TodoRepository(AppDbContext context) : ITodoRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<ResponseGetAllTodoDTO>> GetAllAsync(Guid userId)
    {
        var getTodos = await _context.Todos.Where(u => u.UserId.Equals(userId)).ToListAsync();
        var todos = new List<ResponseGetAllTodoDTO>();
        foreach (var todo in getTodos)
        {
            todos.Add(new ResponseGetAllTodoDTO
            {
                Id = todo.Id,
                Title = todo.Title,
                IsCompleted = todo.IsCompleted,
                CreatedOn = todo.CreatedOn
            });
        }

        return todos;
    }

    public async Task<Todo> Create(RequestCreateTodoDTO request)
    {
        var todo = await _context.Todos.AddAsync(new Todo
        {
            Id = Guid.NewGuid(),
            Title = request.Title
        });
        await _context.SaveChangesAsync();

        return todo.Entity;
    }
}
