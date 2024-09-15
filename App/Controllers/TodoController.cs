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
        var claims = accessTokenValidator.ValidateAndGetClaims(token);
        var todos = await todoRepository.GetAllAsync(claims);

        return Ok(todos);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequestCreateTodoDTO request)
    {
        var token = HttpContext.Request.Headers.Authorization.ToString()["Bearer ".Length..];
        var claims = accessTokenValidator.ValidateAndGetClaims(token);
        var todo = await todoRepository.CreateAsync(request, claims);

        return Ok(todo);
    }

    [HttpDelete("{todoId:guid}")]
    public async Task<IActionResult> Delete(Guid todoId)
    {
        try
        {
            var token = HttpContext.Request.Headers.Authorization.ToString()["Bearer ".Length..];
            var claims = accessTokenValidator.ValidateAndGetClaims(token);
            var result = await todoRepository.DeleteAsync(todoId, claims);
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

    [HttpPut("{todoId:guid}")]
    public async Task<IActionResult> Update(Guid todoId, [FromBody] RequestUpdateTodoTitleDTO request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var result = await todoRepository.UpdateTitleAsync(todoId, request);

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
