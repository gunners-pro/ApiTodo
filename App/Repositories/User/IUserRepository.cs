using ApiTodo.App.DTOs.User;

namespace ApiTodo.App.Repositories.User;

public interface IUserRepository
{
    Task<ResponseLoginUserDTO> Login(RequestLoginUserDTO userLogin);

    Task<ResponseCreateUserDTO> Create(RequestCreateUserDTO newUser);
}