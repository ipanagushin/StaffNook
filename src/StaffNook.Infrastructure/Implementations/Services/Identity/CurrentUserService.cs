using StaffNook.Domain.Dtos;
using StaffNook.Domain.Interfaces.Services.Identity;

namespace StaffNook.Infrastructure.Implementations.Services.Identity;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserModel User { get; private set; }
    public IReadOnlyList<string> Claims { get; private set; }

    public void Set(CurrentUserModel user, IReadOnlyList<string> claims)
    {
        User = user;
        Claims = claims;
    }
}