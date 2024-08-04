using Microsoft.EntityFrameworkCore;

namespace AlturaCMS.DataAccess
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IAsyncRepository<T, TContext> Repository<T>() where T : class;
        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
