using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AlturaCMS.DataAccess;
public static class DependencyInjection
{
    public static IServiceCollection AddAlturaDataAccess<TContext>(
            this IServiceCollection services,
            Action<DatabaseOptions> configureOptions) where TContext : DbContext
    {
        // Configure database options using the provided action
        services.Configure(configureOptions);

        // Register DbContext with the connection string from options
        services.AddDbContext<TContext>((provider, options) =>
        {
            var dbOptions = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            options.UseSqlServer(dbOptions.ConnectionString);
        });

        // Correctly register the repository with DI using a factory
        services.AddScoped(typeof(IAsyncRepository<,>), typeof(EfRepository<,>));

        // Register UnitOfWork
        services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

        return services;
    }
}
