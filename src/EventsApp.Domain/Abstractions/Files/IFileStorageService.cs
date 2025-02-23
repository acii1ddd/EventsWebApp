namespace EventsApp.Domain.Abstractions.Files;

public interface IFileStorageService
{
    /// <summary>
    /// Сохранение картинки в S3 хранилище
    /// </summary>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="mimeType"></param>
    /// <param name="eventId"></param>
    /// <returns>Url картинки</returns>
    public Task<string> UploadAsync(Stream fileStream, string fileName, string mimeType, Guid eventId);

    /// <summary>
    /// Получение ссылки на изображение
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns>Url либо string.Empty</returns>
    public Task<string> GetPreSignedUrl(Guid eventId);
}