using StaffNook.Domain.Dtos.Attachments;

namespace StaffNook.Domain.Interfaces.Services;

public interface IAttachmentsService
{
    public Task<AttachmentDto> GetById(Guid id, CancellationToken cancellationToken = default);
    public Task<Guid> Create(AttachmentCreateDto attachmentCreateDto, CancellationToken cancellationToken = default);
    public Task Update(AttachmentDto attachmentDto, CancellationToken cancellationToken = default);
    public Task Delete(Guid id, CancellationToken cancellationToken = default);
}