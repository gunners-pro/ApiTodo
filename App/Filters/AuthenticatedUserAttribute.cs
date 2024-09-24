using Microsoft.AspNetCore.Mvc;

namespace ApiTodo.App.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute(params string[] Roles) : base(typeof(AuthenticatedUserFilter))
    {
        Arguments = [Roles];
    }
}
