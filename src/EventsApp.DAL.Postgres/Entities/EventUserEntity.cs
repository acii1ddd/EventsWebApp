using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApp.DAL.Entities;

public class EventUserEntity
{
    public Guid EventId { get; set; }

    public EventEntity Event { get; set; } = null!;
    
    public Guid UserId { get; set; }
 
    public UserEntity User { get; set; } = null!;
    
    public DateTime RegisteredAt { get; set; }
}

public class EventUserConfiguration : IEntityTypeConfiguration<EventUserEntity>
{
    public void Configure(EntityTypeBuilder<EventUserEntity> builder)
    {
        builder.ToTable("EventUsers");
        
        builder.HasKey(e => new { e.EventId, e.UserId });
        
        builder.Property(e => e.EventId).IsRequired();
        builder.Property(e => e.UserId).IsRequired();
        
        builder.Property(e => e.RegisteredAt).IsRequired()
            .HasDefaultValueSql("current_timestamp");
    }
}