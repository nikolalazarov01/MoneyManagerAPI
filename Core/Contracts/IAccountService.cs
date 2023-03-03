using System.Linq.Expressions;
using Data.Models;
using Data.Models.DTO.Account;
using Utilities;

namespace Core.Contracts;

public interface IAccountService
{
    Task<OperationResult> ValidateTransaction(AccountInfoRequestDto accountInfoDto, CancellationToken token);
    Task<OperationResult<Account>> MakeTransaction(AccountInfoRequestDto accountInfoDto, CancellationToken token);
    Task<OperationResult<Account>> GetAccount(IEnumerable<Expression<Func<Account, bool>>> filters,
        IEnumerable<Func<IQueryable<Account>, IQueryable<Account>>> transformations, CancellationToken token);
    Task<OperationResult<Account>> AddNewAccountAsync(User user, Account account, CancellationToken token);
    Task<OperationResult> DeleteAccountAsync(Guid id, CancellationToken token);
    Task<OperationResult<IEnumerable<Account>>> GetUserAccounts(Guid userId, CancellationToken token);
}