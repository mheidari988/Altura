using AlturaCMS.Application.Services.Persistence;
using AlturaCMS.DataAccess;
using AlturaCMS.Persistence.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AlturaCMS.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddAlturaApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddAlturaDataAccess<ApplicationDbContext>(options =>
        {
            options.ConnectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string not found");
        });
        services.AddScoped<IContentTypeService, ContentTypeService>();
        services.AddScoped<IFieldService, FieldService>();
        services.AddScoped<IFormFieldService, FormFieldService>();
        services.AddScoped<IFormService, FormService>();
        services.AddScoped<IDynamicTableService, DynamicTableService>();

        return services;
    }
}