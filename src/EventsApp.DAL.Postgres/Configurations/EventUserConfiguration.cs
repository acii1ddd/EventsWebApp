using EventsApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApp.DAL.Configurations;

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