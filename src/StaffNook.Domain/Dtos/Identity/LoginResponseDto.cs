namespace StaffNook.Domain.Dtos.Identity;

public class LoginResponseDto
{
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
}