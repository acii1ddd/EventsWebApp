using EventsApp.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

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
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

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