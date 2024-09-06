using ApiTodo.App.Context;
using ApiTodo.App.DTOs.Todo;
using ApiTodo.App.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiTodo.App.Repositories.Todos;

public class TodoRepository(AppDbContext context) : ITodoRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<Todo>> Get()
    {
        var todos = await _context.Todos.ToListAsync();

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
