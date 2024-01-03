using StaffNook.Domain.Dtos.Identity;

namespace StaffNook.Domain.Interfaces.Services.Identity;

public interface IIdentityService
{
    Task<LoginResponseDto> Login(LoginRequestDto requestDto, CancellationToken cancellationToken = default);
    Task ChangePassword(ChangePasswordRequestDto requestDto);
    Task<CurrentUserResponseDto> GetCurrentUserInfo();
}