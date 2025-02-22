using EventsApp.API.ConfigurationDI;
using EventsApp.BLL.ConfigurationDI;
using EventsApp.DAL;
using EventsApp.DAL.ConfigurationDI;
using EventsApp.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EventsApp.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new ApplicationException("Не удалось получить строку подключения для регистрации контекста базы данных");
                
            options.UseNpgsql(connectionString);
        });

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
            configuration.Enrich.FromLogContext();
        });

        // registration
        builder.Services
            .RegisterRepositories()
            .RegisterDalProfiles()
            .RegisterServices()
            .AddContractProfiles();
            
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // first
        app.UseAuthentication();
        // second
        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //context.Database.Migrate();
            await DbInitializer.Initialize(context);
        }
        
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            app.Run();
        }
        catch (Exception e)
        {
            logger.LogError("Произошла ошибка при работе приложения {error}", e);
        }
    }
}