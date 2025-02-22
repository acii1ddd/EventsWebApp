using EventsApp.DAL.Repositories;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Abstractions.Participants;
using Microsoft.Extensions.DependencyInjection;

namespace EventsApp.DAL.ConfigurationDI;

public static class ConfigurationExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        return services;
    }

    public static IServiceCollection RegisterDalProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(ConfigurationExtensions).Assembly);
        });
        return services;
    }
}