using StaffNook.Domain.Dtos;

namespace StaffNook.Domain.Interfaces.Services.Identity;

public interface ICurrentUserService
{
    CurrentUserModel User { get; }
    IReadOnlyList<string> Claims { get; }
    void Set(CurrentUserModel user, IReadOnlyList<string> claims);
}