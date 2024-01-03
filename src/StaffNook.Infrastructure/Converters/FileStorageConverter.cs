using StaffNook.Domain.Common;
using StaffNook.Domain.Dtos.Attachments;
using StaffNook.Domain.Entities.Attachment;
using StaffNook.Infrastructure.Configuration;

namespace StaffNook.Infrastructure.Converters;

public static class FileStorageConverter
{
    public static string GetBucketName(FileStorageBucket bucket)
    {
        return bucket switch
        {
            FileStorageBucket.Temp => AppConfiguration.FileStorage.TempBucket,
            FileStorageBucket.Permanent => AppConfiguration.FileStorage.PermanentBucket,
            _ => throw new ArgumentOutOfRangeException(nameof(bucket), bucket, null)
        };
    }

    public static FileDto? FileDtoFromAttachment(AttachmentEntity? attachment)
    {
        if (attachment is null)
        {
            return null;
        }
        
        return new FileDto
        {
            Id = attachment.Id,
            Bucket = attachment.Bucket,
            Name = attachment.UniqueFileName,
            Path = attachment.Path,
            BucketName = GetBucketName(attachment.Bucket),
            PreviewUrl = AppConfiguration.FileStorage.PreviewUrl
        };
    }
}