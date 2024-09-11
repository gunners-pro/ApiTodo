using System.Net;
using ApiTodo.App.Attributes;
using ApiTodo.App.DTOs.Todo;
using ApiTodo.App.Repositories.Todos;
using ApiTodo.App.Security.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodo.App.Controllers;

[ApiController]
[Route("[controller]")]
[AuthenticatedUser]
public class TodoController(ITodoRepository todoRepository, IAccessTokenValidator accessTokenValidator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var token = HttpContext.Request.Headers.Authorization.ToString()["Bearer ".Length..];
        var userId = accessTokenValidator.ValidateAndGetUserId(token);
        var todos = await todoRepository.GetAllAsync(userId);

        return Ok(todos);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequestCreateTodoDTO request)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString()["Bearer ".Length..];
        var userId = accessTokenValidator.ValidateAndGetUserId(token);
        var todo = await todoRepository.CreateAsync(request, userId);

        return Ok(todo);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await todoRepository.DeleteAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            var result = new
            {
                statusCode = HttpStatusCode.NotFound,
                message = ex.Message
            };
            return NotFound(result);
        }
    }
}
