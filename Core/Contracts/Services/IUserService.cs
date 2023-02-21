using Data.Models;
using Data.Models.DTO;
using Utilities;

namespace Core.Contracts.Services;

public interface IUserService
{
    Task<OperationResult<User>> RegisterAsync(RegisterRequestDto registerRequestDto);
    Task<OperationResult<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
    Task<OperationResult> DeleteAsync(Guid id);
    Task<OperationResult<User>> GetById(Guid id);
}