using EventsApp.BLL.Services;
using EventsApp.BLL.Services.Auth;
using EventsApp.Domain.Abstractions.Auth;
using EventsApp.Domain.Abstractions.Events;
using EventsApp.Domain.Abstractions.Files;
using EventsApp.Domain.Abstractions.Users;
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
}