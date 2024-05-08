using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.Project;
using StaffNook.Domain.Filters;

namespace StaffNook.Domain.Interfaces.Services;

public interface IProjectService
{
    Task CreateProject(CreateProjectDto createProjectDto, CancellationToken cancellationToken = default);
    Task UpdateProject(Guid id, UpdateProjectDto updateProjectDto, CancellationToken cancellationToken = default);
    Task<PaginationResult<ProjectInfoDto>> GetByPageFilter(ProjectPageFilter pageFilter = default, CancellationToken cancellationToken = default);

    Task UpdateProjectEmployees(Guid projectId, IEnumerable<ProjectEmployeeDto> projectEmployees,
        CancellationToken cancellationToken = default);
}