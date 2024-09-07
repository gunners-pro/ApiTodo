namespace ApiTodo.App.DTOs.Todo;

public class ResponseGetAllTodoDTO
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateOnly CreatedOn { get; set; }
}
