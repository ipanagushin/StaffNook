namespace StaffNook.Domain.Dtos.User;

public class CreateUserDto
{
    /// <summary>
    /// Логин
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Роль
    /// </summary>
    public string Role { get; set; }
    
    /// <summary>
    /// Клеймы для пользователя
    /// </summary>
    public string[] Claims { get; set; }
}