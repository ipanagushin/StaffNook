using StaffNook.Domain.Common;

namespace StaffNook.Domain.Dtos.Attachments;

public class FileDto
{
    public Guid Id { get; set; }
    public FileStorageBucket Bucket { get; set; }
    public string BucketName { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public string PreviewUrl { get; set; }
}