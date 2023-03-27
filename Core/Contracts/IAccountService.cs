using System.Linq.Expressions;
using Core.Contracts.Options;
using Data.Models;
using Data.Models.DTO.Account;
using Utilities;

namespace Core.Contracts;

public interface IAccountService
{
    Task<OperationResult> ValidateTransaction(AccountInfoRequestDto accountInfoDto, CancellationToken token);
    Task<OperationResult<Account>> MakeTransaction(AccountInfoRequestDto accountInfoDto, CancellationToken token);
    Task<OperationResult<Account>> GetAccount(IQueryOptions<Account> queryOptions, CancellationToken token);
    Task<OperationResult<Account>> AddNewAccountAsync(User user, Account account, CancellationToken token);
    Task<OperationResult> DeleteAccountAsync(Guid id, CancellationToken token);
    Task<OperationResult<IEnumerable<Account>>> GetUserAccounts(Guid userId, CancellationToken token);
}