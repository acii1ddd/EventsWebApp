using EventsApp.DAL.Repositories;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Abstractions.Files;
using EventsApp.Domain.Abstractions.Participants;
using EventsApp.Domain.Abstractions.RefreshTokens;
using Microsoft.Extensions.DependencyInjection;

namespace EventsApp.DAL.ConfigurationDI;

public static class ConfigurationExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFileStorageRepository, FileStorageRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
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