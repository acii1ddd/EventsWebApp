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
    
    private const string BucketName = "event-pictures";

    public FileStorageService(IAmazonS3 s3Client, IFileStorageRepository fileStorageRepository, 
        ILogger<FileStorageService> logger)
    {
        _s3Client = s3Client;
        _fileStorageRepository = fileStorageRepository;
        _logger = logger;
    }

    private static string GetPath(string bucketName, string key)
    { 
        return $"{BucketName}/{key}";
    }
    
    public async Task<string> UploadAsync(Stream fileStream, string fileName, string mimeType, Guid eventId)
    {
        var fileId = Guid.NewGuid();

        // Размер необходимо зафиксировать до того, как будет вычитан поток
        // который освободится после прочтения
        var streamLength = fileStream.Length;

        await _s3Client.PutObjectAsync(new PutObjectRequest
        {
            Key = fileId.ToString(),
            BucketName = BucketName,
            InputStream = fileStream,
            ContentType = mimeType
        });

        var imageFile = new ImageFileModel
        {
            Id = fileId,
            BucketName = BucketName,
            StoragePath = GetPath(BucketName, fileId.ToString()),
            Name = fileName,
            Length = streamLength,
            MimeType = mimeType,
            Extension = Path.GetExtension(fileName),
            UploadDate = DateTime.UtcNow,
            EventId = eventId
        };

        _ = await _fileStorageRepository.AddAsync(imageFile);
        _logger.LogInformation("Файл {fileName} успешно добавлен в хранилище", imageFile.Name);
        return await GetPreSignedUrl(imageFile);
    }
    
    public async Task<string> GetPreSignedUrl(ImageFileModel? imageFile)
    {
        if (imageFile is null) return string.Empty;
        
        var url = new GetPreSignedUrlRequest
        {
            BucketName = BucketName,
            Key = imageFile.Id.ToString(),
            Expires = DateTime.UtcNow.AddMinutes(10),
            Protocol = Protocol.HTTP
        };
        
        var imageUrl = await _s3Client.GetPreSignedURLAsync(url);
        return imageUrl;
    }

    public async Task<string> UpdateAsync(Stream fileStream, string fileName, string mimeType, Guid eventId)
    {
        var oldImageFile = await _fileStorageRepository.GetByEventIdAsync(eventId);
        
        var fileId = oldImageFile?.Id ?? Guid.NewGuid();
        
        var streamLength = fileStream.Length;
        
        // обновляем изображение в minio
        await _s3Client.PutObjectAsync(new PutObjectRequest
        {
            Key = fileId.ToString(),
            BucketName = BucketName,
            InputStream = fileStream,
            ContentType = mimeType
        });
        
        var newImageFile = new ImageFileModel
        {
            Id = fileId,
            BucketName = BucketName,
            StoragePath = GetPath(BucketName, fileId.ToString()),
            Name = fileName,
            Length = streamLength,
            MimeType = mimeType,
            Extension = Path.GetExtension(fileName),
            UploadDate = DateTime.UtcNow,
            EventId = eventId
        };
        
        // добавим или обновим изображение
        _ = oldImageFile is null
            ? await _fileStorageRepository.AddAsync(newImageFile)
            : await _fileStorageRepository.UpdateAsync(newImageFile);
        
        return await GetPreSignedUrl(newImageFile);
    }

    public async Task DeleteAsync(ImageFileModel? imageFile)
    {
        if (imageFile is null) return;
        
        // удаляем изображение из minio
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = BucketName,
            Key = imageFile.Id.ToString()
        };
        await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        
        // удаляем из бд
        _ = await _fileStorageRepository.DeleteByIdAsync(imageFile.Id);
        
        _logger.LogInformation("Изображение {imageId} успешно удалено", imageFile.Id);
    }
}
