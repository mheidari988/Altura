using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;

namespace AlturaCMS.Application.Services.Persistence.Common
{
    public abstract class BasePersistenceService<T, TContext>(IUnitOfWork<TContext> unitOfWork, ILogger<BasePersistenceService<T, TContext>>? logger = null) : IBasePersistenceService<T>
        where T : class
        where TContext : DbContext
    {
        protected readonly IUnitOfWork<TContext> UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        protected readonly ILogger<BasePersistenceService<T, TContext>>? Logger = logger;

        public async Task<T?> GetByIdAsync(Guid id)
        {
            var entityName = typeof(T).Name;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                Logger?.LogInformation("Fetching {EntityName} by ID: {EntityId}", entityName, id);
                var entity = await UnitOfWork.Repository<T>().GetByIdAsync(id);
                stopwatch.Stop();
                Logger?.LogInformation("Fetched {EntityName} by ID: {EntityId} in {ElapsedMilliseconds}ms", entityName, id, stopwatch.ElapsedMilliseconds);
                return entity;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Logger?.LogError(ex, "Failed to fetch {EntityName} by ID: {EntityId} in {ElapsedMilliseconds}ms", entityName, id, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var entityName = typeof(T).Name;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                Logger?.LogInformation("Fetching all {EntityName} entities", entityName);
                var entities = await UnitOfWork.Repository<T>().ListAllAsync();
                stopwatch.Stop();
                Logger?.LogInformation("Fetched all {EntityName} entities in {ElapsedMilliseconds}ms", entityName, stopwatch.ElapsedMilliseconds);
                return entities;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Logger?.LogError(ex, "Failed to fetch all {EntityName} entities in {ElapsedMilliseconds}ms", entityName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task<IReadOnlyList<T>> GetByCriteriaAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? skip = null,
            int? take = null)
        {
            var entityName = typeof(T).Name;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                Logger?.LogInformation("Fetching {EntityName} entities by criteria", entityName);
                var entities = await UnitOfWork.Repository<T>().ListAsync(predicate, include, orderBy, skip, take);
                stopwatch.Stop();
                Logger?.LogInformation("Fetched {EntityName} entities by criteria in {ElapsedMilliseconds}ms", entityName, stopwatch.ElapsedMilliseconds);
                return entities;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Logger?.LogError(ex, "Failed to fetch {EntityName} entities by criteria in {ElapsedMilliseconds}ms", entityName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task<IReadOnlyList<T>> GetBySpecificationAsync(ISpecification<T> spec)
        {
            var entityName = typeof(T).Name;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                Logger?.LogInformation("Fetching {EntityName} entities by specification", entityName);
                var entities = await UnitOfWork.Repository<T>().ListAsync(spec);
                stopwatch.Stop();
                Logger?.LogInformation("Fetched {EntityName} entities by specification in {ElapsedMilliseconds}ms", entityName, stopwatch.ElapsedMilliseconds);
                return entities;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Logger?.LogError(ex, "Failed to fetch {EntityName} entities by specification in {ElapsedMilliseconds}ms", entityName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task<T> CreateAsync(T entity)
        {
            var entityName = typeof(T).Name;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                Logger?.LogInformation("Creating {EntityName}: {Entity}", entityName, entity);
                var newEntity = await UnitOfWork.Repository<T>().AddAsync(entity);
                await UnitOfWork.CompleteAsync();
                stopwatch.Stop();
                Logger?.LogInformation("{EntityName} created successfully: {EntityId} in {ElapsedMilliseconds}ms", entityName, newEntity, stopwatch.ElapsedMilliseconds);
                return newEntity;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Logger?.LogError(ex, "Failed to create {EntityName}: {Entity} in {ElapsedMilliseconds}ms", entityName, entity, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            var entityName = typeof(T).Name;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                Logger?.LogInformation("Updating {EntityName}: {Entity}", entityName, entity);
                await UnitOfWork.Repository<T>().UpdateAsync(entity);
                await UnitOfWork.CompleteAsync();
                stopwatch.Stop();
                Logger?.LogInformation("{EntityName} updated successfully: {EntityId} in {ElapsedMilliseconds}ms", entityName, entity, stopwatch.ElapsedMilliseconds);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                stopwatch.Stop();
                Logger?.LogError(ex, "Concurrency conflict while updating {EntityName}: {EntityId} in {ElapsedMilliseconds}ms", entityName, entity, stopwatch.ElapsedMilliseconds);
                throw new ConcurrencyException("A concurrency conflict occurred.", ex);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Logger?.LogError(ex, "Failed to update {EntityName}: {EntityId} in {ElapsedMilliseconds}ms", entityName, entity, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entityName = typeof(T).Name;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                Logger?.LogInformation("Deleting {EntityName} by ID: {EntityId}", entityName, id);
                var entity = await UnitOfWork.Repository<T>().GetByIdAsync(id);
                if (entity != null)
                {
                    await UnitOfWork.Repository<T>().DeleteAsync(entity);
                    await UnitOfWork.CompleteAsync();
                    stopwatch.Stop();
                    Logger?.LogInformation("{EntityName} deleted successfully: {EntityId} in {ElapsedMilliseconds}ms", entityName, id, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    stopwatch.Stop();
                    Logger?.LogWarning("{EntityName} with ID: {EntityId} not found in {ElapsedMilliseconds}ms", entityName, id, stopwatch.ElapsedMilliseconds);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                stopwatch.Stop();
                Logger?.LogError(ex, "Concurrency conflict while deleting {EntityName}: {EntityId} in {ElapsedMilliseconds}ms", entityName, id, stopwatch.ElapsedMilliseconds);
                throw new ConcurrencyException("A concurrency conflict occurred.", ex);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Logger?.LogError(ex, "Failed to delete {EntityName}: {EntityId} in {ElapsedMilliseconds}ms", entityName, id, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
