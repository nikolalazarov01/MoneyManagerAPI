using System.Linq.Expressions;
using Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Data.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _db;

    public Repository(DbContext db)
    {
        _db = db;
    }

    public async Task<OperationResult> CreateAsync(T entity, CancellationToken token)
    {
        var operationResult = new OperationResult();

        if (operationResult.ValidateNotNull(entity) == false) return operationResult;

        try
        {
            await this._db.AddAsync(entity, token);
            await this._db.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    public async Task<OperationResult> DeleteAsync(T entity, CancellationToken token)
    {
        var operationResult = new OperationResult();

        if (operationResult.ValidateNotNull(entity) == false) return operationResult;

        try
        {
            this._db.Remove(entity);
            await this._db.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    public Task<OperationResult> UpdateAsync(T entity, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<T>> GetAsync(IEnumerable<Expression<Func<T, bool>>> func, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<IEnumerable<T>>> GetManyAsync(IEnumerable<Expression<Func<T, bool>>> func, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<bool>> AnyAsync(IEnumerable<Expression<Func<T, bool>>> func, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}