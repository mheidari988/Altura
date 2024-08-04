using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AlturaCMS.Application.Services.Persistence.Common
{
    public abstract class BasePersistenceService<T, TContext> : IBasePersistenceService<T>
        where T : class
        where TContext : DbContext
    {
        protected readonly IUnitOfWork<TContext> UnitOfWork;

        public BasePersistenceService(IUnitOfWork<TContext> unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

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
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
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
            try
            {
                var newEntity = await UnitOfWork.Repository<T>().AddAsync(entity);
                await UnitOfWork.CompleteAsync();
                return newEntity;
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "An error occurred while creating the entity.");
                throw new ApplicationException("An error occurred while creating the entity.", ex);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                await UnitOfWork.Repository<T>().UpdateAsync(entity);
                await UnitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("A concurrency conflict occurred.", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await UnitOfWork.Repository<T>().GetByIdAsync(id);
            if (entity != null)
            {
                try
                {
                    await UnitOfWork.Repository<T>().DeleteAsync(entity);
                    await UnitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw new ConcurrencyException("A concurrency conflict occurred.", ex);
                }
            }
        }
    }
}
