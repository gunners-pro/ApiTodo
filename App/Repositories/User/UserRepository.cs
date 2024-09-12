using ApiTodo.App.Context;
using ApiTodo.App.DTOs.User;
using ApiTodo.App.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace ApiTodo.App.Repositories.User;

public class UserRepository(
    AppDbContext context,
    IAccessTokenGenerator jwtToken
    ) : IUserRepository
{
    private readonly AppDbContext _context = context;
    private readonly IAccessTokenGenerator _jwtToken = jwtToken;

    public async Task<ResponseLoginUserDTO> Login(RequestLoginUserDTO userLogin)
    {
        var userLoginResult = await _context.Users.Where(u => u.Email == userLogin.Email).FirstOrDefaultAsync()
            ?? throw new Exception("The email provided doesn't exists.");

        var validPassword = BC.Verify(userLogin.Password, userLoginResult.Password);
        if (validPassword is false)
            throw new Exception("The password provided doesn't match.");

        var userClaims = new JwtTokenGeneratorDTO
        {
            UserId = userLoginResult.Id,
            Role = userLoginResult.Role
        };

        var user = new ResponseLoginUserDTO()
        {
            Id = userLoginResult.Id,
            Email = userLoginResult.Email,
            Token = _jwtToken.Generate(userClaims)
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

            var userClaims = new JwtTokenGeneratorDTO
            {
                UserId = newUser.Id,
                Role = newUser.Role
            };

            var userResponse = new ResponseCreateUserDTO
            {
                Id = newUser.Id,
                Email = newUser.Email,
                Token = _jwtToken.Generate(userClaims)
            };

            return userResponse;
        }
        else
        {
            throw new Exception("Email already exists.");
        }
    }

    public async Task<bool> GetUserById(Guid userId)
    {
        var user = await _context.Users.AnyAsync(u => u.Id.Equals(userId));
        return user;
    }
}
