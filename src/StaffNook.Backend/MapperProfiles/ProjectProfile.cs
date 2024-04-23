using AutoMapper;
using StaffNook.Domain.Dtos.Project;
using StaffNook.Domain.Entities.Project;

namespace StaffNook.Backend.MapperProfiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<CreateProjectDto, ProjectEntity>();
        CreateMap<ProjectContactDto, ProjectContactsEntity>();
        CreateMap<ProjectRoleDto, ProjectRoleEntity>();
        
        CreateMap<ProjectEntity, ProjectInfoDto>();
        CreateMap<ProjectContactsEntity, ProjectContactDto>();
        CreateMap<ProjectRoleEntity, ProjectRoleDto>();
        CreateMap<ProjectEmployeeEntity, ProjectEmployeeDto>();
    }
}