using Data.DtoModels;
using Data.Models;

namespace Data.Repository.Contracts;

public interface IUserRepository
{
    Task<bool> IsUnique(string username);
    Task<LoginResponseDtoModel> Login(LoginRequestDtoModel loginRequestDto);
    Task<User> Register(RegisterRequestDtoModel registerRequestDto);
}