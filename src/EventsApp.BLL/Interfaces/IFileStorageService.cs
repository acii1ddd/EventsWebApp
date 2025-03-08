using EventsApp.Domain.Models.Images;

namespace EventsApp.BLL.Interfaces;

public interface IFileStorageService
{
    /// <summary>
    /// Сохранение изображения в S3 хранилище
    /// </summary>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="mimeType"></param>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Url изображения</returns>
    public Task<string> UploadAsync(Stream fileStream, string fileName, string mimeType, Guid eventId, 
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение ссылки на изображение
    /// </summary>
    /// <param name="imageFile"></param>
    /// <returns>Url либо string.Empty</returns>
    public Task<string> GetPreSignedUrl(ImageFileModel imageFile);

    /// <summary>
    /// Обновление изображения
    /// </summary>
    /// <param name="fileStream"></param>
    /// <param name="fileName"></param>
    /// <param name="mimeType"></param>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Url нового изображения</returns>
    public Task<string> UpdateAsync(Stream fileStream, string fileName, string mimeType,
        Guid eventId, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление изображения 
    /// </summary>
    /// <param name="imageFile"></param>
    /// <param name="cancellationToken"></param>
    public Task DeleteAsync(ImageFileModel? imageFile, CancellationToken cancellationToken);
}