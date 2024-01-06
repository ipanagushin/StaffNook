namespace StaffNook.Domain.Dtos.Identity;

public class ChangePasswordRequestDto
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Текущий пароль
    /// </summary>
    public string CurrentPassword { get; set; }
    
    /// <summary>
    /// Новый пароль
    /// </summary>
    public string NewPassword { get; set; }
}