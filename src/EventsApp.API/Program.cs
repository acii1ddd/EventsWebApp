using System.Security.Claims;
using System.Text;
using Amazon.Runtime;
using Amazon.S3;
using EventsApp.API.ConfigurationDI;
using EventsApp.API.Extension;
using EventsApp.BLL.ConfigurationDI;
using EventsApp.Configuration;
using EventsApp.DAL;
using EventsApp.DAL.ConfigurationDI;
using EventsApp.DAL.Context;
using EventsApp.Domain.Models.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        
        // configure S3
        builder.Services.Configure<S3Options>(builder.Configuration.GetSection("S3Options"));
        builder.Services.AddSingleton<IAmazonS3>(sp =>
        {
            var awsOptions = sp.GetRequiredService<IOptions<S3Options>>().Value;
            
            var config = new AmazonS3Config
            {
                //RegionEndpoint = RegionEndpoint.GetBySystemName(awsOptions.Region),
                ServiceURL = awsOptions.ServiceUrl,
                UseHttp = awsOptions.UseHttp,
                ForcePathStyle = awsOptions.ForcePathStyle // пути через / (для minio)
            };
            var credentials = new BasicAWSCredentials(awsOptions.AccessKey, awsOptions.SecretKey);
            
            var s3Client = new AmazonS3Client(credentials, config);
            // блокир основной поток для создания бакета
            EnsureBucketExistsAsync(s3Client).GetAwaiter().GetResult();
                
            return s3Client;
        });

        // registration
        builder.Services
            .RegisterRepositories()
            .RegisterDalProfiles()
            .RegisterServices()
            .AddContractProfiles()
            .AddSwagger(); // jwt token int header 
        
        // auth settings
        var authSection = builder.Configuration.GetSection("AuthSettings")
            ?? throw new ApplicationException($"Секция {nameof(AuthSettings)} не найдена в конфигурационном файле");
                
        builder.Services.Configure<AuthSettings>(authSection);
        
        var authSettings = authSection.Get<AuthSettings>() 
            ?? throw new InvalidOperationException("Не удалось сопоставить параметры аутентификации");
        
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Secret)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization(authOptions =>
        {
            authOptions.AddPolicy
            (
                "Default, Admin",
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.Default), nameof(UserRole.Admin));
                }
            );
            
            authOptions.AddPolicy
            (
                nameof(UserRole.Admin),
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.Admin));
                }
            );
        });
        
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
            await app.RunAsync();
        }
        catch (Exception e)
        {
            logger.LogError("Произошла ошибка при работе приложения {error}", e);
        }
    }

    /// <summary>
    /// Создание бакета если его еще нету 
    /// </summary>
    /// <param name="s3Client"></param>
    private static async Task EnsureBucketExistsAsync(AmazonS3Client s3Client)
    {
        const string bucketName = "event-pictures";
        try
        {
            await s3Client.GetBucketLocationAsync(bucketName);
        }
        catch (AmazonS3Exception e) when(e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await s3Client.PutBucketAsync(bucketName);
        }
    }
}