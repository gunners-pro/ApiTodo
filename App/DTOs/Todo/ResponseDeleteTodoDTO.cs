namespace ApiTodo.App.DTOs.Todo;

public class ResponseDeleteTodoDTO
{
    public bool Deleted { get; set; }
    public string Message { get; set; } = string.Empty;
}
