using StaffNook.Domain.Dtos.Identity;

namespace StaffNook.Domain.Interfaces.Services.Identity;

public interface IRoleService
{
    Task<RoleInfoDto> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<RoleInfoDto>> GetAll(CancellationToken cancellationToken = default);
    Task Edit(Guid id, EditRoleDto dto, CancellationToken cancellationToken = default);
    Task<RoleInfoDto> Create(EditRoleDto dto, CancellationToken cancellationToken = default);
}