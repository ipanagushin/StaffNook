using Microsoft.EntityFrameworkCore;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Repositories;

public class UserRepository : Repository<UserEntity>, IUserRepository
{
    public UserRepository(Context context) : base(context)
    {
    }

    public async Task<UserEntity?> GetByLogin(string login, CancellationToken cancellationToken = default)
    {
        return await GetDataSet().FirstOrDefaultAsync(x => x.Login == login, cancellationToken);
    }
}