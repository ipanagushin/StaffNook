using StaffNook.Domain.Entities.Identity;

namespace StaffNook.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepository<UserEntity>
{
    public Task<UserEntity> GetByLogin(string login, CancellationToken cancellationToken = default);
}