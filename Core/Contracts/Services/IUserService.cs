using Data.Models.DTO;
using Utilities;

namespace Core.Contracts.Services;

public interface IUserService
{
    Task<OperationResult<UserDto>> RegisterAsync(RegisterRequestDto registerRequestDto);
    Task<OperationResult<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
}