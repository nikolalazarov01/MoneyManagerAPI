using System.Linq.Expressions;
using Core.Contracts.Options;
using Data.Models.Contracts;

namespace Core.Options;

public class QueryOptions<T> : IQueryOptions<T> where T : class, IEntity
{
    private readonly List<Expression<Func<T, bool>>> _filters = new();
    private readonly List<Func<IQueryable<T>, IQueryable<T>>> _transformations = new();

    public IReadOnlyCollection<Expression<Func<T, bool>>> Filters => this._filters.AsReadOnly();

    public IReadOnlyCollection<Func<IQueryable<T>, IQueryable<T>>> Transformations =>
        this._transformations.AsReadOnly();

    public bool AddFilter(Expression<Func<T, bool>>? filter)
    {
        if (filter is null) return false;
        
        this._filters.Add(filter);
        return true;
    }

    public bool AddTransformation(Func<IQueryable<T>, IQueryable<T>>? transformation)
    {
        if (transformation is null) return false;
        
        this._transformations.Add(transformation);
        return true;
    }
}