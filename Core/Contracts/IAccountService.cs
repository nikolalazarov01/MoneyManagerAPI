using Data.Models;
using Utilities;

namespace Core.Contracts;

public interface IAccountService
{
    Task<OperationResult<Account>> AddNewAccountAsync(User user, Account account, CancellationToken token);
    Task<OperationResult> DeleteAccountAsync(Guid id, CancellationToken token);
}