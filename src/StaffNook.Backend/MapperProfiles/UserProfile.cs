using AutoMapper;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.Attachments;
using StaffNook.Domain.Dtos.Identity;
using StaffNook.Domain.Dtos.User;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Infrastructure.Converters;

namespace StaffNook.Backend.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, UserEntity>();
        CreateMap<UserEntity, UserInfoDto>()
            .ForMember(dest => dest.Attachment,
                opt => 
                    opt.MapFrom(src => FileStorageConverter.FileDtoFromAttachment(src.Attachment)));
        // CreateMap<UserEntity, CurrentUserResponseDto>()
        //     .ForMember(x=>x.);
    }
}

// class CustomResolver : IValueResolver<UserEntity, UserInfoDto, FileDto?>
// {
//     public FileDto? Resolve(UserEntity source, UserInfoDto destination, FileDto? destMember, ResolutionContext context)
//     {
//         return FileStorageConverter.FileDtoFromAttachment(source.Attachment);
//     }
// }
