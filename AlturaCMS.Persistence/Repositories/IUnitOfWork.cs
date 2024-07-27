using AlturaCMS.Domain.Common;

namespace AlturaCMS.Persistence.Repositories
{
    /// <summary>
    /// Represents a unit of work that encapsulates a set of operations that should be executed as a single transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
        /// <returns>The repository for the specified entity type.</returns>
        IAsyncRepository<T> Repository<T>() where T : BaseEntity;

        /// <summary>
        /// Saves all changes made within the unit of work asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> CompleteAsync();
    }
}
