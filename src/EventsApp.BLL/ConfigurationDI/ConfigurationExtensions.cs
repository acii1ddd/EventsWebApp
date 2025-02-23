using EventsApp.BLL.Services;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Abstractions.Files;
using Microsoft.Extensions.DependencyInjection;

namespace EventsApp.BLL.ConfigurationDI;

public static class ConfigurationExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        return services;
    }
}