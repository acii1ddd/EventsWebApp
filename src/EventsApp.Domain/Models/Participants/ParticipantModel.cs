using EventsApp.Domain.Models.Events;

namespace EventsApp.Domain.Models;

public class ParticipantModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Surname { get; set; } = string.Empty;
    
    public DateTime BirthDate { get; set; }
    
    public DateTime EventRegistrationDate { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    /// <summary>
    /// События этого участника
    /// </summary>
    public List<EventModel> Events { get; set; } = [];
}