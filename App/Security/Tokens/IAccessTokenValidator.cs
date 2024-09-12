using System.Security.Claims;

namespace ApiTodo.App.Security.Tokens;

public interface IAccessTokenValidator
{
    IEnumerable<Claim> ValidateAndGetClaims(string token);
}
