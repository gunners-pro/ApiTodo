namespace ApiTodo.App.DTOs.User;

public class RequestCreateUserDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
