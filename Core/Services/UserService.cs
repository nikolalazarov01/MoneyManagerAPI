using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Core.Contracts;
using Data.Models;
using Data.Models.DTO;
using Data.Repository.Contracts;
using Utilities;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;

    public UserService(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<Currency>> SetUserBaseCurrency(Currency currency, Guid userId, CancellationToken token)
    {
        var operationResult = new OperationResult<Currency>();
        try
        {
            var result =
                await _repository.GetAsync(new List<Expression<Func<User, bool>>> { u => u.Id == userId }, token);
            if (!result.IsSuccessful) return operationResult.AppendErrors(result);

            User user;
            if (result.Data is not null)
            {
                user = result.Data;
                user.BaseCurrency = currency;

                var updateResult = await this._repository.UpdateAsync(user, token);
                if (!updateResult.IsSuccessful) return operationResult.AppendErrors(updateResult);
            }
            else
            {
                user = new User();
                user.BaseCurrency = currency;

                var createResult = await this._repository.CreateAsync(user, token);
                if (!createResult.IsSuccessful) return operationResult.AppendErrors(createResult);
            }
            
            operationResult.Data = currency;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }
}