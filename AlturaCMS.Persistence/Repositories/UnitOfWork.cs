using AlturaCMS.Domain.Common;
using AlturaCMS.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;

namespace AlturaCMS.Persistence.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public IAsyncRepository<T> Repository<T>() where T : BaseEntity
    {
        return _serviceProvider.GetService<IAsyncRepository<T>>() ?? throw new ArgumentNullException($"Repository for {typeof(T).Name} not found.");
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}