using Data.Models.DTO;

namespace Data.Repository.Contracts;

public interface IUserRepository
{
    bool IsUnique(string username);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
}