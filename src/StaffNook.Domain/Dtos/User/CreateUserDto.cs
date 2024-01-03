namespace StaffNook.Domain.Dtos.User;

public class CreateUserDto
{
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; }
        
    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; }
    
    /// <summary>
    /// Отчество
    /// </summary>
    public string MiddleName { get; set; }
    
    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Почта
    /// </summary>
    public string Email { get; set; }
        
    /// <summary>
    /// Номер телефона
    /// </summary>
    public string PhoneNumber { get; set; }
    
    /// <summary>
    /// Вложение (фото пользователя)
    /// </summary>
    public Guid AttachmentId { get; set; }
    
    /// <summary>
    /// Роль
    /// </summary>
    public Guid RoleId { get; set; }
}