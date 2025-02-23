using EventsApp.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.DAL;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();
    }
}