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