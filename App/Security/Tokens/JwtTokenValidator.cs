
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ApiTodo.App.Security.Tokens;

public class JwtTokenValidator(IConfiguration configuration) : IAccessTokenValidator
{
    public Guid ValidateAndGetUserId(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!));

        var validationParameter = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = securityKey
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, validationParameter, out _);
        var userId = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        return Guid.Parse(userId);
    }
}
