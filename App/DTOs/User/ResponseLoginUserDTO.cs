namespace ApiTodo.App.DTOs.User;

public class ResponseLoginUserDTO
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
}
