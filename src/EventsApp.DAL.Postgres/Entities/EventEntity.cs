using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    public ImageFileEntity ImageFileEntity { get; set; } = null!;
    
    // Не используется в связях
    //public Guid ImageId { get; set; }
    
    /// <summary>
    /// Участники события
    /// </summary>
    public List<ParticipantEntity> Participants { get; set; } = [];
}

public class EventEntityConfiguration : IEntityTypeConfiguration<EventEntity>
{
    private const int MaxLength = 100;
    
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.ToTable("Events");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.StartDate).IsRequired();
        
        builder.Property(x => x.Location)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.MaxParticipants).IsRequired();

        // связь с таблицей участников
        builder.HasMany(x => x.Participants)
            .WithMany(x => x.Events);
        
        // связь с таблицей картинок
        builder.HasOne(x => x.ImageFileEntity)
            .WithOne(x => x.Event)
            .HasForeignKey<ImageFileEntity>(x => x.EventId);
    }
}