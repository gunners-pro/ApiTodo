namespace ApiTodo.App.Entities;

public class Todo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateOnly CreatedOn { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public Guid UserId { get; set; }
}
