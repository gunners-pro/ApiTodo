namespace ApiTodo.App.Security.Tokens;

public record JwtTokenGeneratorDTO
{
    public Guid UserId { get; set; }
    public string Role { get; set; } = "User";
    public string Email { get; set; } = string.Empty;
}
