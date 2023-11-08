using AutoMapper;
using StaffNook.Domain.Dtos.Employee;
using StaffNook.Domain.Entities.Employee;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services;

namespace StaffNook.Infrastructure.Implementations.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(
        IEmployeeRepository employeeRepository, 
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task Create(CreateEmployeeDto dto)
    {
        var entity = _mapper.Map<EmployeeEntity>(dto);
        await _employeeRepository.Insert(entity);
    }
}