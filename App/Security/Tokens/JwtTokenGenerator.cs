
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ApiTodo.App.Security.Tokens;

public class JwtTokenGenerator(IConfiguration configuration) : IAccessTokenGenerator
{
    public string Generate(JwtTokenGeneratorDTO UserClaims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Sid, UserClaims.UserId.ToString()),
            new(ClaimTypes.Role, UserClaims.Role)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(configuration["Jwt:TokenValidityInMinutes"]!)),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
