using AlturaCMS.Application.Services.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AlturaCMS.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<IContentTypeService, ContentTypeService>();
        services.AddScoped<IFieldService, FieldService>();
        services.AddScoped<IFormFieldService, FormFieldService>();
        services.AddScoped<IFormService, FormService>();
        services.AddScoped<IDynamicTableService, DynamicTableService>();

        return services;
    }
}