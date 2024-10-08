using System.Net;
using ApiTodo.App.Attributes;
using ApiTodo.App.DTOs.User;
using ApiTodo.App.Repositories.User;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodo.App.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    [AuthenticatedUser(["Admin"])]
    public async Task<IActionResult> GetAll()
    {
        var users = await userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(RequestLoginUserDTO request)
    {
        try
        {
            var userLogin = await userRepository.Login(request);
            return Ok(userLogin);
        }
        catch (Exception e)
        {
            var result = new
            {
                StatusCode = HttpStatusCode.NotFound,
                e.Message
            };
            return NotFound(result);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(RequestCreateUserDTO request)
    {
        try
        {
            var userCreate = await userRepository.Create(request);
            return Ok(userCreate);
        }
        catch (Exception e)
        {
            var result = new
            {
                statusCode = HttpStatusCode.Conflict,
                message = e.Message
            };
            return Conflict(result);
        }
    }
}
