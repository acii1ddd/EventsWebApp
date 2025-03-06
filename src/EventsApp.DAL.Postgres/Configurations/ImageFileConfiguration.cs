using EventsApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApp.DAL.Configurations;

public class ImageFileConfiguration : IEntityTypeConfiguration<ImageFileEntity>
{
    private const int MaxLength = 100;
    public void Configure(EntityTypeBuilder<ImageFileEntity> builder)
    {
        builder.ToTable("ImageFiles");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.BucketName)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.StoragePath)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.Length).IsRequired();

        builder.Property(x => x.MimeType)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(x => x.Extension)
            .IsRequired()
            .HasMaxLength(MaxLength);

        builder.Property(x => x.UploadDate).IsRequired();
        
        builder.HasIndex(x => x.EventId).IsUnique();
        
        // связь с таблицей событий
        builder.HasOne(x => x.Event)
            .WithOne(x => x.ImageFile)
            .HasForeignKey<ImageFileEntity>(x => x.EventId);
    }
}