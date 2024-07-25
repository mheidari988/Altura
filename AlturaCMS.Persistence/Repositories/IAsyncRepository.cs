using AlturaCMS.Domain.Common;
using AlturaCMS.Domain.Specifications;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AlturaCMS.Persistence.Repositories
{
    /// <summary>
    /// Represents an asynchronous repository for managing entities.
    /// </summary>
    /// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets a read-only list of all entities asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of entities.</returns>
        Task<IReadOnlyList<T>> ListAllAsync();

        /// <summary>
        /// Gets a read-only list of entities that match the specified criteria asynchronously.
        /// </summary>
        /// <param name="spec">The specification that defines the criteria for selecting entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of entities that match the specified criteria.</returns>
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
            int? skip = null,
            int? take = null);

        /// <summary>
        /// Adds a new entity to the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added entity.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity in the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity from the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(T entity);
    }
}
