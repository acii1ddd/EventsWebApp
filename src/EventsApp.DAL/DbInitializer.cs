using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;

namespace EventsApp.DAL;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        if (context.Events.Any())
        {
            return;
        }

        var eventEntity = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Event1",
            Description = "Test Description1",
            StartDate = DateTime.UtcNow,
            Location = "Test Location1",
            Category = "Test Category1",
            MaxParticipants = 50,
            ImageId = Guid.NewGuid(),
        };

        await context.Events.AddAsync(eventEntity);
        await context.SaveChangesAsync();
    }
}