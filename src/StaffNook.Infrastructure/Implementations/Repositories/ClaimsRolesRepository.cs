using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class ClaimsRolesRepository : Repository<ClaimsRolesEntity>, IClaimsRolesRepository
{
    public ClaimsRolesRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }
    
    public Task<ClaimsRolesEntity[]> GetByRoleId(Guid roleId, CancellationToken cancellationToken = default)
    {
        return GetDataSet().Where(x => x.RoleId == roleId && x.IsArchived == false).ToArrayAsync(cancellationToken);
    }
}