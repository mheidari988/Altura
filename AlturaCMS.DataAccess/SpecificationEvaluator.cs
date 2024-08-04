using Microsoft.EntityFrameworkCore;

namespace AlturaCMS.DataAccess;

/// <summary>
/// Evaluates specifications to apply them to queryable entity sets.
/// </summary>
/// <typeparam name="T">The type of the entity to which the specification applies.</typeparam>
public class SpecificationEvaluator<T> where T : class
{
    /// <summary>
    /// Applies the specification to the queryable entity set.
    /// </summary>
    /// <param name="inputQuery">The initial queryable entity set.</param>
    /// <param name="spec">The specification that defines the criteria for selecting entities.</param>
    /// <returns>The modified queryable entity set.</returns>
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        // Modify the IQueryable using the specification's criteria expression
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        // Includes all expression-based includes
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        // Apply ordering if expressions are set
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        // Apply paging if enabled
        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    }
}