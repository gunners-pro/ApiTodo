namespace ApiTodo.App.DTOs.User;

public class ResponseGetUsersDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}
