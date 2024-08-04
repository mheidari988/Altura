using System.Linq.Expressions;

namespace AlturaCMS.DataAccess
{
    /// <summary>
    /// Represents a specification pattern for querying entities.
    /// </summary>
    /// <typeparam name="T">The type of the entity to which the specification applies.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets the criteria for the specification.
        /// </summary>
        Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Gets the list of include expressions for the specification.
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Gets the expression for ordering the results in ascending order.
        /// </summary>
        Expression<Func<T, object>> OrderBy { get; }

        /// <summary>
        /// Gets the expression for ordering the results in descending order.
        /// </summary>
        Expression<Func<T, object>> OrderByDescending { get; }

        /// <summary>
        /// Gets the number of records to take.
        /// </summary>
        int Take { get; }

        /// <summary>
        /// Gets the number of records to skip.
        /// </summary>
        int Skip { get; }

        /// <summary>
        /// Gets a value indicating whether paging is enabled.
        /// </summary>
        bool IsPagingEnabled { get; }
    }
}
