using AlturaCMS.Domain.Common;
using AlturaCMS.Persistence.Repositories;
using AlturaCMS.Persistence.Specifications;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AlturaCMS.Application.Services.Persistence.Common;
public abstract class BasePersistenceService<T>(IUnitOfWork unitOfWork) : IBasePersistenceService<T> where T : BaseEntity
{
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await UnitOfWork.Repository<T>().GetByIdAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await UnitOfWork.Repository<T>().ListAllAsync();
    }

    public async Task<IReadOnlyList<T>> GetByCriteriaAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
        int? skip = null,
        int? take = null)
    {
        return await UnitOfWork.Repository<T>().ListAsync(predicate, include, orderBy, skip, take);
    }

    public async Task<IReadOnlyList<T>> GetBySpecificationAsync(ISpecification<T> spec)
    {
        return await UnitOfWork.Repository<T>().ListAsync(spec);
    }

    public async Task<T> CreateAsync(T entity)
    {
        var newEntity = await UnitOfWork.Repository<T>().AddAsync(entity);
        await UnitOfWork.CompleteAsync();
        return newEntity;
    }

    public async Task UpdateAsync(T entity)
    {
        await UnitOfWork.Repository<T>().UpdateAsync(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await UnitOfWork.Repository<T>().GetByIdAsync(id);
        if (entity != null)
        {
            await UnitOfWork.Repository<T>().DeleteAsync(entity);
            await UnitOfWork.CompleteAsync();
        }
    }
}
