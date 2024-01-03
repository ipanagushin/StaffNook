#nullable enable
using StaffNook.Domain.Common;
using StaffNook.Domain.Entities.Attachment;

namespace StaffNook.Domain.Dtos.Attachments;

public class AttachmentDto
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
    
    /// <summary>
    /// Дата добавления
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public static AttachmentDto ToModel(AttachmentEntity entity)
    {
        return new AttachmentDto
        {
            Id = entity.Id,
            Bucket = entity.Bucket,
            Path = entity.Path,
            CreatedAt = entity.CreatedAt,
            UniqueFileName = entity.UniqueFileName
        };
    }
    /// <summary>
    /// Удален ли файл
    /// </summary>
    public bool IsDeleted { get; set; }

    public static AttachmentDto? FromEntity(AttachmentEntity? entity)
    {
        if (entity is null)
        {
            return null;
        }
        return new AttachmentDto
        {
            Bucket = entity.Bucket,
            Id = entity.Id,
            Path = entity.Path,
            CreatedAt = entity.CreatedAt,
            UniqueFileName = entity.UniqueFileName
        };
    }
}