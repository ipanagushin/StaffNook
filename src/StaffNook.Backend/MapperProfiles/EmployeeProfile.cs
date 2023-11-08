using AutoMapper;
using StaffNook.Domain.Dtos.Employee;
using StaffNook.Domain.Entities.Employee;

namespace StaffNook.Backend.MapperProfiles;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<CreateEmployeeDto, EmployeeEntity>();
    }
}