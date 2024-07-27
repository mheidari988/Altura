using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlturaCMS.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }
}
