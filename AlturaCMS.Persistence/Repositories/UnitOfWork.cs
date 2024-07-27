using AlturaCMS.Domain.Common;
using AlturaCMS.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;

namespace AlturaCMS.Persistence.Repositories
{
    /// <summary>
    /// Represents a unit of work that encapsulates a set of operations that should be executed as a single transaction.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The service provider for resolving dependencies.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
        public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
        /// <returns>The repository for the specified entity type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the repository for the specified entity type is not found.</exception>
        public IAsyncRepository<T> Repository<T>() where T : BaseEntity
        {
            return _serviceProvider.GetService<IAsyncRepository<T>>() ?? throw new ArgumentNullException($"Repository for {typeof(T).Name} not found.");
        }

        /// <summary>
        /// Saves all changes made within the unit of work asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the unit of work and releases any resources held by the database context.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
