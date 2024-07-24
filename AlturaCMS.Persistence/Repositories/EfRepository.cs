using AlturaCMS.Domain.Common;
using AlturaCMS.Domain.Specifications;
using AlturaCMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AlturaCMS.Persistence.Repositories;

public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _dbContext;

    public EfRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).OriginalValues["RowVersion"] = entity.RowVersion;
        _dbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
    }
}

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        // modify the IQueryable using the specification's criteria expression
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        // includes all expression-based includes
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        // apply ordering if expressions are set
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        // apply paging if enabled
        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    }
}
