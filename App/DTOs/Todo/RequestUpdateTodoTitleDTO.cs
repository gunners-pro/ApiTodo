using System.ComponentModel.DataAnnotations;

namespace ApiTodo.App.DTOs.Todo;

public class RequestUpdateTodoTitleDTO
{
    [Required(ErrorMessage = "Campo obrigatório")]
    public required string Title { get; set; }
}
