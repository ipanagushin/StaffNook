using StaffNook.Domain.Entities.Project;
using StaffNook.Domain.Interfaces.Repositories.Project;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories.Project;

public class ProjectContactsRepository : Repository<ProjectContactsEntity>, IProjectContactsRepository
{
    public ProjectContactsRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }
}