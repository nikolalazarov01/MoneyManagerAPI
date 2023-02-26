using Data.Models;
using Utilities;

namespace Core.Contracts;

public interface IAccountService
{
    Task<OperationResult<Account>> AddNewAccount(User user, Account account, CancellationToken token);
}