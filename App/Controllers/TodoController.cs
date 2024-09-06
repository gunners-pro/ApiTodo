using ApiTodo.App.DTOs.Todo;
using ApiTodo.App.Repositories.Todos;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodo.App.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController(ITodoRepository todoRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var todos = await todoRepository.Get();

        return Ok(todos);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequestCreateTodoDTO request)
    {
        var todo = await todoRepository.Create(request);

        return Ok(todo);
    }
}
