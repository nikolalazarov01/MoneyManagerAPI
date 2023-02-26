using Core.Contracts;
using Data.Models;
using Data.Models.DTO.Account;
using Data.Repository.Contracts;
using Utilities;

namespace Core.Services;

public class AccountService : IAccountService
{
    private readonly IRepository<Account> _repository;

    public AccountService(IRepository<Account> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<Account>> AddNewAccount(User user, Account account, CancellationToken token)
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
}