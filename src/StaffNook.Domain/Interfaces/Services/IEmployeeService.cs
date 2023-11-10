using StaffNook.Domain.Dtos.Employee;

namespace StaffNook.Domain.Interfaces.Services;

public interface IEmployeeService
{
    Task<EmployeeInfoDto> GetById(Guid id);
    Task Create(CreateEmployeeDto dto);
    // Task Update(UpdateEmployeeDto employeeEntity);
}