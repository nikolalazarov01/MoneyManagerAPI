using System.Linq.Expressions;
using Data.Extensions;
using Data.Models.Contracts;
using Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Data.Repository;

public class Repository<T> : IRepository<T> where T : class, IEntity
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

    public async Task<OperationResult> UpdateAsync(T entity, CancellationToken token)
    {
        var operationResult = new OperationResult();

        if (operationResult.ValidateNotNull(entity) == false) return operationResult;

        try
        {
            var trackedEntity = this._db.Set<T>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (trackedEntity != null) this._db.Entry(trackedEntity).State = EntityState.Detached;
            this._db.Entry(entity).State = EntityState.Modified;

            this._db.Update(entity);
            await this._db.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    public async Task<OperationResult<T>> GetAsync(IEnumerable<Expression<Func<T, bool>>> func, CancellationToken token)
    {
        var operationResult = new OperationResult<T>();

        try
        {
            var result = await this._db.Set<T>().Filter(func).FirstOrDefaultAsync(token);
            operationResult.Data = result;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
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