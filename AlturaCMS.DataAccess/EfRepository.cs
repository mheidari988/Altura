using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Reflection;

namespace AlturaCMS.DataAccess;

/// <summary>
/// Represents a repository for managing entities using Entity Framework.
/// </summary>
/// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
/// <typeparam name="TContext">The type of the DbContext used by the repository.</typeparam>
public class EfRepository<T, TContext> : IAsyncRepository<T, TContext>
    where T : class
    where TContext : DbContext
{
    /// <summary>
    /// The database context.
    /// </summary>
    protected readonly TContext DbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="EfRepository{T, TContext}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public EfRepository(TContext dbContext)
    {
        DbContext = dbContext;
    }

    /// <summary>
    /// Gets an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbContext.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Gets a read-only list of all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of entities.</returns>
    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await DbContext.Set<T>().ToListAsync();
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
    /// Gets a read-only list of entities that match the specified criteria asynchronously.
    /// </summary>
    /// <param name="predicate">The expression that defines the criteria for selecting entities.</param>
    /// <param name="include">The function to include related entities.</param>
    /// <param name="orderBy">The function to order the entities.</param>
    /// <param name="skip">The number of entities to skip.</param>
    /// <param name="take">The number of entities to take.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of entities that match the specified criteria.</returns>
    public async Task<IReadOnlyList<T>> ListAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? skip = null,
        int? take = null)
    {
        IQueryable<T> query = DbContext.Set<T>();

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (skip.HasValue && take.HasValue)
        {
            query = query.Skip(skip.Value).Take(take.Value);
        }

        return await query.ToListAsync();
    }

    /// <summary>
    /// Adds a new entity to the repository asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added entity.</returns>
    public async Task<T> AddAsync(T entity)
    {
        await DbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// Updates an existing entity in the repository asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task UpdateAsync(T entity)
    {
        // Use reflection to check for the RowVersion property
        var rowVersionProperty = typeof(T).GetProperty("RowVersion", BindingFlags.Public | BindingFlags.Instance);

        if (rowVersionProperty != null && rowVersionProperty.PropertyType == typeof(byte[]))
        {
            // Get the RowVersion value
            var rowVersionValue = rowVersionProperty.GetValue(entity) as byte[];

            if (rowVersionValue != null)
            {
                DbContext.Entry(entity).OriginalValues["RowVersion"] = rowVersionValue;
            }
        }

        DbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Deletes an entity from the repository asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task DeleteAsync(T entity)
    {
        DbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Applies the specification to the queryable entity set.
    /// </summary>
    /// <param name="spec">The specification that defines the criteria for selecting entities.</param>
    /// <returns>The modified queryable entity set.</returns>
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(DbContext.Set<T>().AsQueryable(), spec);
    }
}
