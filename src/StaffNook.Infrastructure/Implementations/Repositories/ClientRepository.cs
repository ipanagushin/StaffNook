using StaffNook.Domain.Entities.Client;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class ClientRepository : Repository<ClientEntity>, IClientRepository
{
    public ClientRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }
}