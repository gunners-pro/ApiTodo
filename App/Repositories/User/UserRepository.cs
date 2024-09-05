using ApiTodo.App.Context;
using ApiTodo.App.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace ApiTodo.App.Repositories.User;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ResponseLoginUserDTO> Login(RequestLoginUserDTO userLogin)
    {

        var userLoginResult = await _context.Users.Where(u => u.Email == userLogin.Email && u.Password == userLogin.Password).FirstOrDefaultAsync()
            ?? throw new Exception("Email or Password is wrong");

        var user = new ResponseLoginUserDTO()
        {
            Id = userLoginResult.Id,
            Email = userLoginResult.Email,
            Token = "12345678"
        };

        return user;
    }
}
