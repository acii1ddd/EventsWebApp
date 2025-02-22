using Amazon.S3;
using Amazon.S3.Model;
using EventsApp.Domain.Abstractions.Files;
using EventsApp.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EventsApp.BLL.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly IFileStorageRepository _fileStorageRepository;
    private readonly ILogger<FileStorageService> _logger;
    
    private const string BucketName = "EventPictures";

    public FileStorageService(IAmazonS3 s3Client, IFileStorageRepository fileStorageRepository, ILogger<FileStorageService> logger)
    {
        _s3Client = s3Client;
        _fileStorageRepository = fileStorageRepository;
        _logger = logger;
    }

    private static string GetKey(Guid fileId, Guid eventId)
    { 
        return $"{eventId.ToString()}/{fileId.ToString()}";
    }
    
    public async Task<ImageFileModel> UploadAsync(Stream fileStream, string fileName, string mimeType, Guid eventId)
    {
        var fileId = Guid.NewGuid();
        var key = GetKey(eventId, fileId);
        
        // Размер необходимо зафиксировать до того, как будет вычитан поток
        // который освободится после прочтения
        var streamLength = fileStream.Length;

        await _s3Client.PutObjectAsync(new PutObjectRequest
        {
            Key = key,
            BucketName = BucketName,
            InputStream = fileStream,
            ContentType = mimeType
        });

        var imageFile = new ImageFileModel
        {
            Id = fileId,
            BucketName = BucketName,
            StoragePath = $"{BucketName}/{key}",
            Name = fileName,
            Length = streamLength,
            MimeType = mimeType,
            Extension = Path.GetExtension(fileName),
            UploadDate = DateTime.UtcNow,
            EventId = eventId
        };

        var result = await _fileStorageRepository.AddAsync(imageFile);
        _logger.LogInformation("Файл {fileName} успешно добавлен в хранилище", imageFile.Name);
        
        return result;
    }
}
