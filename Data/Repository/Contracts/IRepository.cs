using System.Linq.Expressions;
using Utilities;

namespace Data.Repository.Contracts;

public interface IRepository<T> where T : class
{
    Task<OperationResult> CreateAsync(T entity, CancellationToken token);
    Task<OperationResult> DeleteAsync(T entity, CancellationToken token);
    Task<OperationResult> UpdateAsync(T entity, CancellationToken token);
    Task<OperationResult<T>> GetAsync(IEnumerable<Expression<Func<T, bool>>> func, IEnumerable<Func<IQueryable<T>, IQueryable<T>>> transforms, CancellationToken token);
    Task<OperationResult<IEnumerable<T>>> GetManyAsync(IEnumerable<Expression<Func<T, bool>>> func,
        CancellationToken token);
    Task<OperationResult<bool>> AnyAsync(IEnumerable<Expression<Func<T, bool>>> func, CancellationToken token);
}