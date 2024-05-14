using StaffNook.Domain.Entities.Employee;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class WorkingTimeRepository : Repository<WorkingTimeEntity>, IWorkingTimeRepository
{
    public WorkingTimeRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }
}