using AlturaCMS.Application.Services.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlturaCMS.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IContentTypeService, ContentTypeService>();
        services.AddScoped<IFieldService, FieldService>();
        services.AddScoped<IFormFieldService, FormFieldService>();
        services.AddScoped<IFormService, FormService>();
        services.AddScoped<IValidationRulesService, ValidationRulesService>();

        return services;
    }
}