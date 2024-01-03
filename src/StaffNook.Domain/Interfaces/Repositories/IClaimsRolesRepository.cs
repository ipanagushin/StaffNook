using StaffNook.Domain.Entities.Identity;

namespace StaffNook.Domain.Interfaces.Repositories;

public interface IClaimsRolesRepository : IRepository<ClaimsRolesEntity>
{
    public Task<ClaimsRolesEntity[]> GetByRoleId(Guid roleId, CancellationToken cancellationToken = default);
}