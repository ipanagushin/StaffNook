using StaffNook.Domain.Dtos.Identity;
using StaffNook.Domain.Dtos.User;

namespace StaffNook.Domain.Interfaces.Services;

public interface IIdentityService
{
    Task<LoginResponseDto> Login(LoginRequestDto requestDto);
    Task Create(CreateUserDto dto);
    Task<string[]> GetUserClaims(Guid id);
}