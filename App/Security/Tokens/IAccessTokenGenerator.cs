namespace ApiTodo.App.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(JwtTokenGeneratorDTO UserClaims);
}
