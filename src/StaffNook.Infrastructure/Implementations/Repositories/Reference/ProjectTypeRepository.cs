using StaffNook.Domain.Entities.Reference;
using StaffNook.Domain.Interfaces.Repositories.Reference;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories.Reference;

public class ProjectTypeRepository : Repository<ProjectTypeEntity>, IProjectTypeRepository
{
    public ProjectTypeRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }
}