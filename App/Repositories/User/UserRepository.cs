using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ApiTodo.App.Context;
using ApiTodo.App.DTOs.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace ApiTodo.App.Repositories.User;

public class UserRepository(AppDbContext context, IConfiguration configuration) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ResponseLoginUserDTO> Login(RequestLoginUserDTO userLogin)
    {
        var userLoginResult = await _context.Users.Where(u => u.Email == userLogin.Email).FirstOrDefaultAsync()
            ?? throw new Exception("The email provided doesn't exists.");

        var validPassword = BC.Verify(userLogin.Password, userLoginResult.Password);
        if (validPassword is false)
            throw new Exception("The password provided doesn't match.");

        var user = new ResponseLoginUserDTO()
        {
            Id = userLoginResult.Id,
            Email = userLoginResult.Email,
            Token = GenerateJWTToken()
        };

        return user;
    }

    public async Task<ResponseCreateUserDTO> Create(RequestCreateUserDTO user)
    {
        var newUser = await _context.Users.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
        if (newUser is null)
        {
            newUser = new Entities.User
            {
                Id = Guid.NewGuid(),
                Email = user.Email,
                Password = BC.HashPassword(user.Password, 6)
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            var userResponse = new ResponseCreateUserDTO
            {
                Id = newUser.Id,
                Email = newUser.Email,
                Token = GenerateJWTToken()
            };

            return userResponse;
        }
        else
        {
            throw new Exception("Email already exists.");
        }
    }

    private string GenerateJWTToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(double.Parse(configuration["Jwt:TokenValidityInMinutes"]!)),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
