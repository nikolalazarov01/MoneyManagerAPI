using System.Linq.Expressions;
using Core.Contracts;
using Data.Models;
using Data.Models.DTO.Account;
using Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Core.Services;

public class AccountService : IAccountService
{
    private readonly IRepository<Account> _repository;

    public AccountService(IRepository<Account> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<Account>> AddNewAccountAsync(User user, Account account, CancellationToken token)
    {
        var operationResult = new OperationResult<Account>();
        try
        {
            if (user.Accounts is null)
            {
                operationResult.AddError(new Error
                    { IsNotExpected = true, Message = "User's accounts are null!" });
                return operationResult;
            }
            user.Accounts.Add(account);
            account.User = user;

            var result = await this._repository.CreateAsync(account, token);
            if (!result.IsSuccessful) return operationResult.AppendErrors(result);

            operationResult.Data = account;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    public async Task<OperationResult> DeleteAccountAsync(Guid id, CancellationToken token)
    {
        var operationResult = new OperationResult();

        try
        {
            var funcs = new List<Expression<Func<Account, bool>>>
            {
                a => a.Id == id
            };
            var account = await this._repository.GetAsync(funcs, null, token);
            if (!account.IsSuccessful) return operationResult.AppendErrors(account);
            if (account.Data is null)
            {
                operationResult.AddError(new Error
                    { IsNotExpected = true, Message = "Account data not found!" });
            }

            var result = await this._repository.DeleteAsync(account.Data, token);
            if (!result.IsSuccessful) return operationResult.AppendErrors(result);
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    public async Task<OperationResult<IEnumerable<Account>>> GetUserAccounts(Guid userId, CancellationToken token)
    {
        var operationResult = new OperationResult<IEnumerable<Account>>();
        try
        {
            var funcs = new List<Expression<Func<Account, bool>>>
            {
                a => a.UserId == userId
            };
            var transforms = new List<Func<IQueryable<Account>, IQueryable<Account>>>
            {
                a => a.Include(ac => ac.Currency)
            };


            var result = await this._repository.GetManyAsync(funcs, transforms, token);
            if (!result.IsSuccessful) return operationResult.AppendErrors(result);

            operationResult.Data = result.Data;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }
}