using System.Linq.Expressions;
using Data.Models.Contracts;

namespace Core.Contracts.Options;

public interface IQueryOptions<T> where T : class, IEntity
{
    IReadOnlyCollection<Expression<Func<T, bool>>> Filters { get; }
    IReadOnlyCollection<Func<IQueryable<T>, IQueryable<T>>> Transformations { get; }
}