using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class UserRepository : Repository<UserEntity>, IUserRepository
{
    public UserRepository(Context context, ICurrentUserService currentUserService) : base(context, currentUserService)
    {
    }
    
    public async Task<UserEntity?> GetByLogin(string login, CancellationToken cancellationToken = default)
    {
        return await GetDataSet().FirstOrDefaultAsync(x => x.Login == login, cancellationToken);
    }
}