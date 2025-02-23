using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApp.DAL.Entities;

public class ImageFileEntity
{
    public Guid Id { get; set; }
    
    public string BucketName { get; set; } = string.Empty;
    
    /// <summary>
    /// Путь к картинке в хранилище S3
    /// </summary>
    public string StoragePath { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public long Length { get; set; }
    
    public string MimeType { get; set; } = string.Empty;
    
    public string Extension { get; set; } = string.Empty;
    
    public DateTime UploadDate { get; set; }
    
    /// <summary>
    /// Событие, связанное с картинкой
    /// </summary>
    public EventEntity Event { get; set; } = null!;
    
    public Guid EventId { get; set; }
}

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