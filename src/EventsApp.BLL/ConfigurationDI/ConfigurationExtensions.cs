using EventsApp.BLL.Interfaces;
using EventsApp.BLL.Interfaces.Auth;
using EventsApp.BLL.Services;
using EventsApp.BLL.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace EventsApp.BLL.ConfigurationDI;

public static class ConfigurationExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHashService, PasswordHashService>();
        return services;
    }
    
    public static IServiceCollection RegisterBllProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(ConfigurationExtensions).Assembly);
        });
        return services;
    }
}