using AutoMapper;
using StaffNook.Domain.Dtos.WorkingTime;
using StaffNook.Domain.Entities.Employee;

namespace StaffNook.Backend.MapperProfiles;

public class WorkingTimeProfile : Profile
{
    public WorkingTimeProfile()
    {
        CreateMap<CreateWorkingTimeDto, WorkingTimeEntity>();
        CreateMap<UpdateWorkingTimeDto, WorkingTimeEntity>();
        CreateMap<WorkingTimeEntity, WorkingTimeInfoDto>();
    }
}