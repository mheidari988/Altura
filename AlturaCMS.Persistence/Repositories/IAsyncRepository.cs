using AlturaCMS.Domain.Common;
using AlturaCMS.Domain.Specifications;

namespace AlturaCMS.Persistence.Repositories;

public interface IAsyncRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}