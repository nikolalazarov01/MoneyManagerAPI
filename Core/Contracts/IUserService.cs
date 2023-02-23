using Data.Models;
using Utilities;

namespace Core.Contracts;

public interface IUserService
{
    Task<OperationResult<User>> GetUserById(Guid id, IEnumerable<Func<IQueryable<User>, IQueryable<User>>> transforms, CancellationToken token);
    Task<OperationResult<User>> SetUserBaseCurrency(Currency currency, Guid userId, CancellationToken token);
}