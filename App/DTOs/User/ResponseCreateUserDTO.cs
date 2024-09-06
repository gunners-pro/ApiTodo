namespace ApiTodo.App.DTOs.User;

public class ResponseCreateUserDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
