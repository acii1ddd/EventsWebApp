using EventsApp.BLL.Services;
using EventsApp.Domain.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventsApp.BLL.ConfigurationDI;

public static class ConfigurationExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();
        return services;
    }
}