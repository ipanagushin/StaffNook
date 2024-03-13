using AutoMapper;
using StaffNook.Domain.Dtos.News;
using StaffNook.Domain.Entities.News;

namespace StaffNook.Backend.MapperProfiles;

public class NewsProfile : Profile
{
    public NewsProfile()
    {
        CreateMap<CreateNewsDto, NewsEntity>();
        CreateMap<UpdateNewsDto, NewsEntity>();
        CreateMap<NewsEntity, NewsInfoDto>();
    }
}