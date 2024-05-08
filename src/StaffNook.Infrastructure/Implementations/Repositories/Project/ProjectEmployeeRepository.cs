using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Entities.Project;
using StaffNook.Domain.Interfaces.Repositories.Project;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories.Project;

public class ProjectEmployeeRepository : Repository<ProjectEmployeeEntity>, IProjectEmployeeRepository
{
    public ProjectEmployeeRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }

    public async Task<IEnumerable<ProjectEmployeeEntity>> GetByProjectId(Guid projectId, CancellationToken cancellationToken = default)
    {
        var projectEmployeeEntities = GetDataSet().Where(x => x.ProjectId == projectId);
        return await projectEmployeeEntities.ToArrayAsync(cancellationToken: cancellationToken);
    }
}