using ApiTodo.App.DTOs.Todo;
using ApiTodo.App.Entities;

namespace ApiTodo.App.Repositories.Todos;

public interface ITodoRepository
{
    Task<List<ResponseGetAllTodoDTO>> GetAllAsync(Guid userId);

    Task<Todo> CreateAsync(RequestCreateTodoDTO request, Guid userId);

    Task<ResponseDeleteTodoDTO> DeleteAsync(Guid todoId);
}
