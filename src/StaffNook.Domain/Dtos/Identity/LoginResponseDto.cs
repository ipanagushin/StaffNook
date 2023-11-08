namespace StaffNook.Domain.Dtos.Identity;

public class LoginResponseDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
}