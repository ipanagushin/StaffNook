namespace StaffNook.Domain.Dtos.Identity;

public class LoginRequestDto
{
    /// <summary>
    /// Логин
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
}