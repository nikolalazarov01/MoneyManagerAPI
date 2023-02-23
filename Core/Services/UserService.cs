using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Core.Contracts;
using Data.Models;
using Data.Models.DTO;
using Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;

    public UserService(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<User>> GetUserById(Guid id, IEnumerable<Func<IQueryable<User>, IQueryable<User>>> transforms, CancellationToken token)
    {
        var operationResult = new OperationResult<User>();
        try
        {
            var result =
                await this._repository.GetAsync(new List<Expression<Func<User, bool>>> { u => u.Id == id },transforms, token);
            if (!result.IsSuccessful) return operationResult.AppendErrors(result);

            return operationResult.WithData(result.Data);
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
            return operationResult;
        }
    }

    public async Task<OperationResult<User>> SetUserBaseCurrency(Currency currency, Guid userId, CancellationToken token)
    {
        var operationResult = new OperationResult<User>();
        try
        {
            var result =
                await _repository.GetAsync(new List<Expression<Func<User, bool>>> { u => u.Id == userId }, null, token);
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
                user.Id = userId;
                user.BaseCurrency = currency;

                var createResult = await this._repository.CreateAsync(user, token);
                if (!createResult.IsSuccessful) return operationResult.AppendErrors(createResult);
            }
            
            operationResult.Data = user;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }
}