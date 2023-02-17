using Data.DtoModels;

namespace Data.Repository.Contracts;

public interface IUserRepository
{
    Task<bool> IsUnique(string username);
    Task<LoginResponseDtoModel> Login(LoginRequestDtoModel loginRequestDto);
}