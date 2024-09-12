using System.Security.Claims;
using ApiTodo.App.Context;
using ApiTodo.App.DTOs.Todo;
using ApiTodo.App.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiTodo.App.Repositories.Todos;

public class TodoRepository(AppDbContext context) : ITodoRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<ResponseGetAllTodoDTO>> GetAllAsync(IEnumerable<Claim> claims)
    {
        var todos = new List<ResponseGetAllTodoDTO>();
        List<Todo> getTodos = [];
        string role = claims.First(c => c.Type == ClaimTypes.Role).Value;
        if (role.Equals("Admin"))
        {
            getTodos = await _context.Todos.ToListAsync();
        }
        else if (role.Equals("User"))
        {
            Guid userId = Guid.Parse(claims.First(c => c.Type == ClaimTypes.Sid).Value);
            getTodos = await _context.Todos.Where(t => t.UserId.Equals(userId)).ToListAsync();
        }

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

    public async Task<Todo> CreateAsync(RequestCreateTodoDTO request, IEnumerable<Claim> claims)
    {
        Guid userId = Guid.Parse(claims.First(c => c.Type == ClaimTypes.Sid).Value);
        var todo = await _context.Todos.AddAsync(new Todo
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            UserId = userId
        });
        await _context.SaveChangesAsync();

        return todo.Entity;
    }

    public async Task<ResponseDeleteTodoDTO> DeleteAsync(Guid todoId)
    {
        var todo = await _context.Todos.FindAsync(todoId) ?? throw new Exception("Todo doesn't exists");
        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();

        return new ResponseDeleteTodoDTO
        {
            Deleted = true,
            Message = "Todo deleted"
        };
    }
}
