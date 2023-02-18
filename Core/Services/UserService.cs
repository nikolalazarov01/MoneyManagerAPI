using Data.Models.DTO;
using Data.Repository;
using Utilities;

namespace Core.Services;

public class UserService
{
    private readonly UserRepository _repository;

    public UserService(UserRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var operationResult = new OperationResult<LoginResponseDto>();
        try
        {
            var result = await this._repository.Login(loginRequestDto.Username, loginRequestDto.Password);

            return !result.IsSuccessful ? operationResult.AppendErrors(result) : operationResult.WithData(result.Data);
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
            return operationResult;
        }
    }
}