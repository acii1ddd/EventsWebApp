using EventsApp.DAL.Context;
using EventsApp.DAL.Entities;

namespace EventsApp.DAL;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        if (!context.Events.Any())
        {
            var eventEntity = new EventEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Event1",
                Description = "Test Description1",
                StartDate = DateTime.UtcNow,
                Location = "Test Location1",
                Category = "Test Category1",
                MaxParticipants = 50,
            };
            await context.Events.AddAsync(eventEntity);
            await context.SaveChangesAsync();
        }
        
        if (!context.Participants.Any())
        {
            var participantEntity = new ParticipantEntity
            {
                Id = Guid.NewGuid(),
                Name = "Test Name",
                Surname = "Test Surname",
                BirthDate = DateTime.UtcNow,
                EventRegistrationDate = DateTime.UtcNow,
                Email = "Test Email"
            };
            await context.Participants.AddAsync(participantEntity);
            await context.SaveChangesAsync();
        }
    }
}