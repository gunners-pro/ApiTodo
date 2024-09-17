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

    public async Task<Todo> UpdateTitleAsync(Guid todoId, RequestUpdateTodoTitleDTO request)
    {
        var todo = await _context.Todos.Where(t => t.Id.Equals(todoId)).FirstOrDefaultAsync()
            ?? throw new Exception("Todo doesn't exists.");

        todo.Title = request.Title;
        _context.Todos.Update(todo);
        await _context.SaveChangesAsync();

        return todo;
    }

    public async Task<ResponseDeleteTodoDTO> DeleteAsync(Guid todoId, IEnumerable<Claim> claims)
    {
        Todo? getTodo = null;

        string role = claims.First(c => c.Type == ClaimTypes.Role).Value;
        if (role.Equals("Admin"))
        {
            getTodo = await _context.Todos.FindAsync(todoId);
            if (getTodo is null)
            {
                throw new Exception("Todo doesn't exists.");
            }
            _context.Todos.Remove(getTodo!);
            await _context.SaveChangesAsync();
        }
        else if (role.Equals("User"))
        {
            var userId = Guid.Parse(claims.First(c => c.Type == ClaimTypes.Sid).Value);

            getTodo = await _context.Todos.Where(t => t.UserId.Equals(userId) && t.Id.Equals(todoId)).FirstOrDefaultAsync();
            if (getTodo is null)
            {
                throw new Exception("Todo doesn't exists.");
            }
            _context.Todos.Remove(getTodo!);
            await _context.SaveChangesAsync();
        }

        return new ResponseDeleteTodoDTO
        {
            Deleted = true,
            Message = "Todo deleted"
        };
    }

    public async Task CompleteTodoAsync(Guid todoId)
    {
        var todo = await _context.Todos.Where(t => t.Id.Equals(todoId)).FirstOrDefaultAsync()
            ?? throw new Exception("Todo doesn't exists.");

        todo.IsCompleted = !todo.IsCompleted;
        _context.Todos.Update(todo);
        await _context.SaveChangesAsync();
    }
}
