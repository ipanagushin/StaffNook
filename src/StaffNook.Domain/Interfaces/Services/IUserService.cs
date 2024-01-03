using StaffNook.Domain.Dtos;
using StaffNook.Domain.Dtos.User;
using StaffNook.Domain.Filters;

namespace StaffNook.Domain.Interfaces.Services;

public interface IUserService
{
    Task Create(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
    Task<UserInfoDto> GetById(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserInfoDto>> GetAll(CancellationToken cancellationToken = default);
    Task<PaginationResult<UserInfoDto>> GetByPageFilter(UserPageFilter pageFilter = default, CancellationToken cancellationToken = default);
}