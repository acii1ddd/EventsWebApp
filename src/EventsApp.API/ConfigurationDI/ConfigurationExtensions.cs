using EventsApp.API.ContractValidators.Events;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace EventsApp.API.ConfigurationDI;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddContractProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(ConfigurationExtensions).Assembly);
        });
        
        services.AddValidatorsFromAssemblyContaining<GetEventByPageRequestValidator>();
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        return services;
    }
}