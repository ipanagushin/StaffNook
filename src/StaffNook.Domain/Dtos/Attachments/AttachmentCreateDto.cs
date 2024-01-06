using StaffNook.Domain.Common;

namespace StaffNook.Domain.Dtos.Attachments;

public class AttachmentCreateDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Имя файла в minio
    /// </summary>
    public string UniqueFileName { get; set; }
    
    /// <summary>
    /// Путь до файла
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Bucket в котором находится файл
    /// </summary>
    public FileStorageBucket Bucket { get; set; }
}