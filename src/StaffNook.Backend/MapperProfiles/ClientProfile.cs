using AutoMapper;
using StaffNook.Domain.Dtos.Client;
using StaffNook.Domain.Entities.Client;

namespace StaffNook.Backend.MapperProfiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<CreateClientDto, ClientEntity>();
        CreateMap<UpdateClientDto, ClientEntity>();
        CreateMap<ClientEntity, ClientInfoDto>();
    }
}