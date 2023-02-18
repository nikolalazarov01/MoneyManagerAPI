using Data.Models;
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

    public async Task<OperationResult<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var operationResult = new OperationResult<LoginResponseDto>();
        try
        {
            var isValid = await this._repository.IsUnique(loginRequestDto.Username);
            if (!isValid.IsSuccessful)
            {
                return operationResult.AppendErrors(isValid);
            }
            
            var result = await this._repository.Login(loginRequestDto.Username, loginRequestDto.Password);

            return !result.IsSuccessful ? operationResult.AppendErrors(result) : operationResult.WithData(result.Data);
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
            return operationResult;
        }
    }

    public async Task<OperationResult<UserDto>> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        var operationResult = new OperationResult<UserDto>();
        try
        {
            User user = new User
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.Email,
                NormalizedEmail = registerRequestDto.Email.ToUpper(),
                BaseCurrency = new Currency(registerRequestDto.Currency.Code)
            };

            var result = await this._repository.Register(user, registerRequestDto.Password);

            return !result.IsSuccessful ? operationResult.AppendErrors(result) : operationResult.WithData(result.Data);
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
            return operationResult;
        }
    }
}