using StaffNook.Domain.Entities.Attachment;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class AttachmentRepository : Repository<AttachmentEntity>, IAttachmentRepository
{
    public AttachmentRepository(Context context) : base(context)
    {
    }
}