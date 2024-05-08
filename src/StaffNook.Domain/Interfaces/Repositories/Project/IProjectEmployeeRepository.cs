using StaffNook.Domain.Entities.Project;

namespace StaffNook.Domain.Interfaces.Repositories.Project;

public interface IProjectEmployeeRepository : IRepository<ProjectEmployeeEntity>
{
    public Task<IEnumerable<ProjectEmployeeEntity>> GetByProjectId(Guid projectId,
        CancellationToken cancellationToken = default);
}