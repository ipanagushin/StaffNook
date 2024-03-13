using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class RoleRepository : Repository<RoleEntity>, IRoleRepository
{
    public RoleRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }
}