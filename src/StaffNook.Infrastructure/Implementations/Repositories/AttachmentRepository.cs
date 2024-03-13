using StaffNook.Domain.Entities.Attachment;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class AttachmentRepository : Repository<AttachmentEntity>, IAttachmentRepository
{
    public AttachmentRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }
}