using ApiTodo.App.DTOs.Todo;
using ApiTodo.App.Entities;

namespace ApiTodo.App.Repositories.Todos;

public interface ITodoRepository
{
    Task<List<Todo>> Get();

    Task<Todo> Create(RequestCreateTodoDTO request);
}
