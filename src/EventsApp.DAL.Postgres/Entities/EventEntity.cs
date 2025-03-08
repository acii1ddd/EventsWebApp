namespace EventsApp.DAL.Entities;

public class EventEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public string Location { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    public int MaxParticipants { get; set; }

    /// <summary>
    /// Изображение события
    /// </summary>
    public ImageFileEntity ImageFile { get; set; } = null!;
    
    /// <summary>
    /// Участники события
    /// </summary>
    // public List<UserEntity> Users { get; set; } = [];
    
    /// <summary>
    /// Связующая таблица с юзерами
    /// </summary>
    public List<EventUserEntity> EventUsers { get; set; } = [];
}