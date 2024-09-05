namespace ApiTodo.App.DTOs.User;

public class RequestLoginUserDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
