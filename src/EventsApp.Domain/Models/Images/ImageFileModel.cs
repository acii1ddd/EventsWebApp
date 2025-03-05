using System;
using EventsApp.Domain.Models.Events;

namespace EventsApp.Domain.Models.Images;

public class ImageFileModel
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
    /// Событие, к которому привязана картинка
    /// </summary>
    public EventModel Event { get; set; } = null!;
    
    public Guid EventId { get; set; }
}