using StaffNook.Domain.Dtos.Employee;

namespace StaffNook.Domain.Interfaces.Services;

public interface IEmployeeService
{
    Task Create(CreateEmployeeDto dto);
    // Task Update(UpdateEmployeeDto employeeEntity);
}