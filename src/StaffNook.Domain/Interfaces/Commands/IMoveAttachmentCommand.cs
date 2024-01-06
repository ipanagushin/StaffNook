using StaffNook.Domain.Common;
using StaffNook.Domain.Dtos.Attachments;

namespace StaffNook.Domain.Interfaces.Commands;

public interface IMoveAttachmentCommand
{
    Task<FileDto> MoveBucket(FileDto file, FileStorageBucket destinationBucket);

    Task<Guid?> GetOrUpdateAttachmentId(FileDto file);
}