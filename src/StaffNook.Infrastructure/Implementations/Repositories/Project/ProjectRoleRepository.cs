using StaffNook.Domain.Entities.Project;
using StaffNook.Domain.Interfaces.Repositories.Project;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories.Project;

public class ProjectRoleRepository : Repository<ProjectRoleEntity>, IProjectRoleRepository
{
    public ProjectRoleRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }
}