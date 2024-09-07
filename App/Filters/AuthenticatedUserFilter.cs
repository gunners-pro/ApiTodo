using System.Net;
using ApiTodo.App.Repositories.User;
using ApiTodo.App.Security.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace ApiTodo.App.Attributes;

public class AuthenticatedUserFilter(
    IAccessTokenValidator accessTokenValidator,
    IUserRepository userRepository
    ) : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator = accessTokenValidator;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);
            var userId = _accessTokenValidator.ValidateAndGetUserId(token);

            var exist = await _userRepository.GetUserById(userId);
            if (exist is false)
            {
                throw new Exception("Usuário sem permissão");
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                statusCode = HttpStatusCode.Unauthorized,
                message = "Token expirado"
            });
        }
        catch (Exception ex)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                statusCode = HttpStatusCode.Unauthorized,
                message = ex.Message
            });
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authentication))
        {
            throw new Exception("Nenhum Token informado");
        }

        return authentication["Bearer ".Length..].Trim();
    }
}
