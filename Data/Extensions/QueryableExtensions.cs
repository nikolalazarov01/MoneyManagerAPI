using System.Linq.Expressions;

namespace Data.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, IEnumerable<Expression<Func<T, bool>>> filters)
    {
        if (queryable is null) throw new ArgumentNullException(nameof(queryable));

        foreach (var filter in filters)
        {
            queryable = queryable.Where(filter);
        }

        return queryable;
    }
}