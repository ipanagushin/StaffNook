using StaffNook.Domain.Common;
using StaffNook.Domain.Dtos.Attachments;
using StaffNook.Domain.Entities.Attachment;
using StaffNook.Domain.Interfaces.Commands;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Infrastructure.Converters;

namespace StaffNook.Infrastructure.Implementations.Commands;

public class MoveAttachmentCommand : IMoveAttachmentCommand
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IAttachmentRepository _attachmentRepository;

    public MoveAttachmentCommand(
        IFileStorageService fileStorageService, 
        IAttachmentRepository attachmentRepository)
    {
        _fileStorageService = fileStorageService;
        _attachmentRepository = attachmentRepository;
    }


    public async Task<Guid?> GetOrUpdateAttachmentId(FileDto file)
    {
        if (file?.Bucket is FileStorageBucket.Temp)
        {
            var newFile = await MoveBucket(file, FileStorageBucket.Permanent);
            return newFile.Id;
        }

        return file?.Id;
    }

    public async Task<FileDto> MoveBucket(FileDto file, FileStorageBucket destinationBucket)
    {
        if (destinationBucket == file.Bucket)
        {
            return file;
        }
        
        var newFile = await _fileStorageService.MoveFile(file.Path, file.Name, file.Bucket, destinationBucket);

        var entity = await _attachmentRepository.Update(new AttachmentEntity
        {
            Id = newFile.Id,
            Bucket = newFile.Bucket,
            Path = newFile.Path,
            CreatedAt = DateTime.UtcNow,
            UniqueFileName = newFile.Name
        });

        return new FileDto
        {
            Id = entity.Id,
            Bucket = entity.Bucket,
            BucketName = FileStorageConverter.GetBucketName(entity.Bucket),
            Name = entity.UniqueFileName,
            Path = entity.Path
        };
    }
}
