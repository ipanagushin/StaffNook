using AutoMapper;
using StaffNook.Domain.Dtos.Identity;
using StaffNook.Domain.Entities.Identity;

namespace StaffNook.Backend.MapperProfiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<EditRoleDto, RoleEntity>();
        CreateMap<RoleEntity, RoleInfoDto>()
            .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.RoleClaims.Select(rc => rc.Claim).ToArray()));
    }
}