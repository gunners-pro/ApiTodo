using System.Net;
using ApiTodo.App.DTOs.User;
using ApiTodo.App.Repositories.User;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodo.App.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
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
                statusCode = HttpStatusCode.NotFound,
                message = e.Message
            };
            return NotFound(result);
        }
    }
}
