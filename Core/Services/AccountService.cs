using System.Linq.Expressions;
using Core.Contracts;
using Data.Models;
using Data.Models.DTO.Account;
using Data.Repository.Contracts;
using Data.Utils;
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

    public async Task<OperationResult> ValidateTransaction(AccountInfoRequestDto accountInfoDto, CancellationToken token)
    {
        var operationResult = new OperationResult();
        try
        {
            var type = this.GetAccountInfoType(accountInfoDto.Type);
            if (!type.IsSuccessful) return operationResult.AppendErrors(type);
            
            if (type.Data is AccountInfoType.Income)
            {
                return operationResult;
            }
            
            var filters = new List<Expression<Func<Account, bool>>>
            {
                a => a.Id == accountInfoDto.AccountId,
                a => a.Total > accountInfoDto.Total
            };
            var result = await this._repository.AnyAsync(filters, token);
            if (!result.IsSuccessful) return operationResult.AppendErrors(result);

            if (!result.Data)
            {
                operationResult.AddError(new Error
                    { Message = "Not enough money in the account to make the transaction!" });
            }
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    public async Task<OperationResult<Account>> MakeTransaction(AccountInfoRequestDto accountInfoDto, CancellationToken token)
    {
        var operationResult = new OperationResult<Account>();
        try
        {
            var filters = new List<Expression<Func<Account, bool>>>
            {
                a => a.Id == accountInfoDto.AccountId
            };
            var transformations = new List<Func<IQueryable<Account>, IQueryable<Account>>>
            {
                a => a.Include(ac => ac.AccountInfos)
            };

            var accountResult = await this._repository.GetAsync(filters, transformations, token);
            if (!accountResult.IsSuccessful) return operationResult.AppendErrors(accountResult);

            var account = accountResult.Data;

            var accountInfo = new AccountInfo
            {
                Account = account,
                Total = accountInfoDto.Total,
                Date = DateTime.Today.ToUniversalTime()
            };
            var parsedType = this.GetAccountInfoType(accountInfoDto.Type);
            if (!parsedType.IsSuccessful) return operationResult.AppendErrors(parsedType);

            accountInfo.Type = parsedType.Data;

            account.AccountInfos.Add(accountInfo);
            switch (parsedType.Data)
            {
                case AccountInfoType.Income:
                    account.Total += accountInfoDto.Total;
                    break;
                case AccountInfoType.Outcome:
                    account.Total -= accountInfoDto.Total;
                    break;
                default: break;
            }

            var updateResult = await this._repository.UpdateAsync(account, token);
            if (!updateResult.IsSuccessful) return operationResult.AppendErrors(updateResult);

            var result = await this._repository.GetAsync(filters, transformations, token);
            if (!result.IsSuccessful) return operationResult.AppendErrors(result);
            
            operationResult.Data = result.Data;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    public async Task<OperationResult<Account>> GetAccount(IEnumerable<Expression<Func<Account, bool>>> filters,
        IEnumerable<Func<IQueryable<Account>, IQueryable<Account>>> transformations, CancellationToken token)
    {
        var operationResult = new OperationResult<Account>();
        try
        {
            var result = await this._repository.GetAsync(filters, transformations, token);
            if (!result.IsSuccessful) return operationResult.AppendErrors(result);

            operationResult.Data = result.Data;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
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
                a => a.Include(ac => ac.Currency),
                a => a.Include(ac => ac.AccountInfos)
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

    private OperationResult<AccountInfoType> GetAccountInfoType(string typeToParse)
    {
        var operationResult = new OperationResult<AccountInfoType>();
        AccountInfoType parsedType = AccountInfoType.Income;
        bool hasParsed = false;

        foreach (AccountInfoType type in Enum.GetValues(typeof(AccountInfoType)))
        {
            if (type.ToString().ToLowerInvariant() == typeToParse.ToLowerInvariant())
            {
                hasParsed = true;
                parsedType = type;
            }
        }

        if (!hasParsed)
        {
            operationResult.AddError(new Error { Message = "Invalid transaction type!" });
        }
        else
        {
            operationResult.Data = parsedType;
        }
        return operationResult;
    }
}