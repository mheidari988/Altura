using AlturaCMS.Domain.Common;
using AlturaCMS.Domain.Specifications;
using AlturaCMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AlturaCMS.Persistence.Repositories
{
    /// <summary>
    /// Represents a repository for managing entities using Entity Framework.
    /// </summary>
    /// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// The database context.
        /// </summary>
        protected readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{T}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public EfRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Gets a read-only list of all entities asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of entities.</returns>
        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Gets a read-only list of entities that match the specified criteria asynchronously.
        /// </summary>
        /// <param name="spec">The specification that defines the criteria for selecting entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of entities that match the specified criteria.</returns>
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        /// <summary>
        /// Adds a new entity to the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added entity.</returns>
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Updates an existing entity in the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).OriginalValues["RowVersion"] = entity.RowVersion;
            _dbContext.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes an entity from the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Applies the specification to the queryable entity set.
        /// </summary>
        /// <param name="spec">The specification that defines the criteria for selecting entities.</param>
        /// <returns>The modified queryable entity set.</returns>
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
    }

    /// <summary>
    /// Evaluates specifications to apply them to queryable entity sets.
    /// </summary>
    /// <typeparam name="T">The type of the entity to which the specification applies.</typeparam>
    public class SpecificationEvaluator<T> where T : BaseEntity
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
}
