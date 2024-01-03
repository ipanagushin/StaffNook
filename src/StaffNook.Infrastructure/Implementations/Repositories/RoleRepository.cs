using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class RoleRepository : Repository<RoleEntity>, IRoleRepository
{
    public RoleRepository(Context context) : base(context)
    {
    }
}