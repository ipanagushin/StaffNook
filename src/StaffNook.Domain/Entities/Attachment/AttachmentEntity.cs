#nullable disable
using StaffNook.Domain.Common;
using StaffNook.Domain.Entities.Base;

namespace StaffNook.Domain.Entities.Attachment;

public class AttachmentEntity : BaseEntity
{
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
    
    /// <summary>
    /// Дата добавления
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}