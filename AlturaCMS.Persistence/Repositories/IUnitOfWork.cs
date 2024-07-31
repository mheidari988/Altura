using AlturaCMS.Domain.Common;

namespace AlturaCMS.Persistence.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAsyncRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
