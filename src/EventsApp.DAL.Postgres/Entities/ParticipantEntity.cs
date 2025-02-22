using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApp.DAL.Entities;

public class ParticipantEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Surname { get; set; } = string.Empty;
    
    public DateTime BirthDate { get; set; }
    
    public DateTime EventRegistrationDate { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// События этого участника
    /// </summary>
    public List<EventEntity> Events { get; set; } = [];
    
    /// <summary>
    /// Refresh token участника
    /// </summary>
    public RefreshTokenEntity RefreshToken { get; set; } = null!;
    
    //public Guid RefreshTokenId { get; set; }
}

public class ParticipantEntityConfiguration : IEntityTypeConfiguration<ParticipantEntity>
{
    private const int MaxLength = 100;
    public void Configure(EntityTypeBuilder<ParticipantEntity> builder)
    {
        builder.ToTable("Participants");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.Surname)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.BirthDate).IsRequired();
        builder.Property(x => x.EventRegistrationDate).IsRequired();
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(MaxLength);
        builder.HasIndex(x => x.Email).IsUnique();
        
        // связь с таблицей событий
        builder.HasMany(x => x.Events)
            .WithMany(x => x.Participants);
        
        // связь с таблицей токенов
        builder.HasOne(x => x.RefreshToken)
            .WithOne(x => x.Participant)
            .HasForeignKey<RefreshTokenEntity>(x => x.ParticipantId);
    }
}