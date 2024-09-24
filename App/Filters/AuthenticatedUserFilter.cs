using System.Net;
using System.Security.Claims;
using ApiTodo.App.Repositories.User;
using ApiTodo.App.Security.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace ApiTodo.App.Attributes;

public class AuthenticatedUserFilter(
    IAccessTokenValidator accessTokenValidator,
    IUserRepository userRepository,
    string[] Roles
    ) : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator = accessTokenValidator;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly string[] _roles = Roles;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);
            var claims = _accessTokenValidator.ValidateAndGetClaims(token);
            Guid userId = Guid.Parse(claims.First(c => c.Type.Equals(ClaimTypes.Sid)).Value);
            var role = claims.First(c => c.Type.Equals(ClaimTypes.Role)).Value;
            if (!_roles.Contains(role))
            {
                throw new Exception("Usuário sem permissão");
            }

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
