using AutoMapper;
using StaffNook.Domain.Dtos.User;
using StaffNook.Domain.Entities.Identity;

namespace StaffNook.Backend.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, UserEntity>();
    }
}