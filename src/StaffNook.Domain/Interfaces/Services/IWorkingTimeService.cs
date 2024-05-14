using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.WorkingTime;
using StaffNook.Domain.Filters;

namespace StaffNook.Domain.Interfaces.Services;

public interface IWorkingTimeService
{
    Task Create(Guid userId, CreateWorkingTimeDto createWorkingTimeDto, CancellationToken cancellationToken = default);
    Task Update(Guid id, UpdateWorkingTimeDto updateWorkingTimeDto, CancellationToken cancellationToken = default);
    Task<WorkingTimeInfoDto> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<PaginationResult<WorkingTimeInfoDto>> GetByPageFilter(WorkingTimePageFilter pageFilter = default, CancellationToken cancellationToken = default);
}