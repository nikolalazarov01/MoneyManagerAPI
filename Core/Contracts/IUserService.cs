using Data.Models;
using Utilities;

namespace Core.Contracts;

public interface IUserService
{
    Task<OperationResult<Currency>> SetUserBaseCurrency(Currency currency, Guid userId, CancellationToken token);
}