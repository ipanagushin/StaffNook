using StaffNook.Domain.Entities.Employee;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class EmployeeRepository : Repository<EmployeeEntity>, IEmployeeRepository
{
    public EmployeeRepository(Context context) : base(context)
    {
    }
}