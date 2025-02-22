namespace EventsApp.API.ConfigurationDI;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddContractProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(ConfigurationExtensions).Assembly);
        });
        return services;
    }
}