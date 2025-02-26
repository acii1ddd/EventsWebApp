using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;
using EventsApp.Domain.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Users.Any())
        {
            // hash TestPassword
            const string testHash = "$2a$11$9kK.gWG7NDghhUDYYGXBy.IzWHPApaGjRSVEfi/G5KKzXW2Qgf7Eq";
            
            var adminEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "admin",
                Surname = "TestSurname",
                BirthDate = DateTime.Parse("03-05-2005").ToUniversalTime(),
                Email = "admin.com",
                PasswordHash = testHash,
                Role = UserRole.Admin
            };
            
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = "user",
                Surname = "TestSurname",
                BirthDate = DateTime.Parse("03-05-2005").ToUniversalTime(),
                Email = "user.com",
                PasswordHash = testHash,
                Role = UserRole.Default
            };
            
            await context.Users.AddAsync(adminEntity);
            await context.Users.AddAsync(userEntity);
            await context.SaveChangesAsync();
        }
    }
}