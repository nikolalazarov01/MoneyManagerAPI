using Data.Models;
using Data.Models.DTO;
using Utilities;

namespace Data.Repository.Contracts;

public interface IUserRepository
{
    Task<OperationResult<bool>> IsUnique(string username);
    Task<OperationResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto);
    Task<OperationResult<UserDto>> Register(RegisterRequestDto registerRequestDto);
}