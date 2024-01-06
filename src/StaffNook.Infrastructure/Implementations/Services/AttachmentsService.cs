using StaffNook.Domain.Dtos.Attachments;
using StaffNook.Domain.Entities.Attachment;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Infrastructure.Exceptions;

namespace StaffNook.Infrastructure.Implementations.Services;

public class AttachmentsService : IAttachmentsService
{
    private readonly IAttachmentRepository _attachmentRepository;

    public AttachmentsService(IAttachmentRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
    }


    public async Task<AttachmentDto> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var entity =
            await _attachmentRepository.GetById(id, cancellationToken);

        if (entity is null)
        {
            throw new Exception($"File {id} not found");
        }

        return new AttachmentDto
        {
            Id = entity.Id,
            Bucket = entity.Bucket,
            Path = entity.Path,
            CreatedAt = entity.CreatedAt,
            UniqueFileName = entity.UniqueFileName
        };
    }

    public async Task<Guid> Create(AttachmentCreateDto attachmentCreateDto, CancellationToken cancellationToken = default)
    {
        var result = await _attachmentRepository.Insert(new AttachmentEntity
        {
            Bucket = attachmentCreateDto.Bucket,
            Path = attachmentCreateDto.Path,
            CreatedAt = DateTime.UtcNow,
            UniqueFileName = attachmentCreateDto.UniqueFileName
        });

        return result.Id;
    }

    public async Task Update(AttachmentDto attachmentDto, CancellationToken cancellationToken = default)
    {
        var entity = 
            await _attachmentRepository.GetById(attachmentDto.Id, cancellationToken);

        if (entity is null)
        {
            throw new Exception($"File {attachmentDto.Id} not found");
        }

        entity.Path = attachmentDto.Path;
        entity.UniqueFileName = attachmentDto.UniqueFileName;
        entity.Bucket = attachmentDto.Bucket;

        await _attachmentRepository.Update(entity, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = 
            await _attachmentRepository.GetById(id, cancellationToken);

        if (entity is null)
        {
            throw NotFoundException.With<AttachmentEntity>(id);
        }

        await _attachmentRepository.Delete(entity, cancellationToken);
    }
}