namespace ApiTodo.App.Security.Tokens;

public interface IAccessTokenValidator
{
    Guid ValidateAndGetUserId(string token);
}
