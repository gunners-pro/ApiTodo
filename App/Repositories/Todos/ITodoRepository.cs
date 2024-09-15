using System.Security.Claims;
using ApiTodo.App.DTOs.Todo;
using ApiTodo.App.Entities;

namespace ApiTodo.App.Repositories.Todos;

public interface ITodoRepository
{
    Task<List<ResponseGetAllTodoDTO>> GetAllAsync(IEnumerable<Claim> claims);

    Task<Todo> CreateAsync(RequestCreateTodoDTO request, IEnumerable<Claim> claims);

    Task<Todo> UpdateTitleAsync(Guid todoId, RequestUpdateTodoTitleDTO request);

    Task<ResponseDeleteTodoDTO> DeleteAsync(Guid todoId, IEnumerable<Claim> claims);
}
