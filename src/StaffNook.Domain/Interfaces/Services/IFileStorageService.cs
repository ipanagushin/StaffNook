using StaffNook.Domain.Common;
using StaffNook.Domain.Dtos.Attachments;

namespace StaffNook.Domain.Interfaces.Services;

public interface IFileStorageService
{
    /// <summary>
    /// Получение url для файла
    /// </summary>
    /// <param name="attachmentId"></param>
    /// <returns></returns>
    Task<string> GetPreviewUrl(Guid attachmentId);
    
    /// <summary>
    /// Получение dto для файла
    /// </summary>
    /// <param name="attachmentId"></param>
    /// <returns></returns>
    Task<FileDto> GetFileDto(Guid attachmentId);
    
    /// <summary>
    /// Записать файла в бакет
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="content"></param>
    /// <param name="fileName"></param>
    /// <param name="bucket"></param>
    /// <returns></returns>
    Task<FileDto> Store(string filePath, Stream content, string fileName, FileStorageBucket bucket);
    
    /// <summary>
    /// Записать файла в бакет
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="content"></param>
    /// <param name="filePath"></param>
    /// <param name="bucket"></param>
    /// <returns></returns>
    Task<FileDto> Store(string filePath, byte[] content, string fileName, FileStorageBucket bucket);
    
    /// <summary>
    /// Удалить файл из бакета
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileKey"></param>
    /// <param name="bucket"></param>
    /// <returns></returns>
    Task Delete(string filePath, string fileKey, FileStorageBucket bucket);
    
    /// <summary>
    /// Удалить файл по идентификатору attachment
    /// </summary>
    /// <param name="attachmentId"></param>
    /// <returns></returns>
    Task DeleteByAttachmentId(Guid attachmentId);
    
    /// <summary>
    /// Удалить файл по идентификатору attachment (временно)
    /// </summary>
    /// <param name="attachmentId"></param>
    /// <returns></returns>
    Task TempDeleteByAttachmentId(Guid attachmentId);
    
    /// <summary>
    /// Переместить файл из одного бакета в другой по идентификатору attachment 
    /// </summary>
    /// <param name="attachmentId"></param>
    /// <param name="destinationBucket"></param>
    /// <param name="newFileName"></param>
    /// <returns></returns>
    Task<FileDto> MoveFileByAttachmentId(Guid attachmentId, FileStorageBucket destinationBucket, string newFileName = null);
    
    /// <summary>
    /// Переместить файл из одного бакета в другой
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileKey"></param>
    /// <param name="sourceBucket"></param>
    /// <param name="destinationBucket"></param>
    /// <param name="newFileName"></param>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<FileDto> MoveFile(string filePath, string fileKey, FileStorageBucket sourceBucket, FileStorageBucket destinationBucket, string newFileName = null);
    
    /// <summary>
    /// Генерирует уникальное имя для каждого файла
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string RandomizeFileName(string fileName)
    {
        return $"{DateTime.Now.Ticks}.{Guid.NewGuid():N}{Path.GetExtension(fileName)}".ToLower();
    }
}