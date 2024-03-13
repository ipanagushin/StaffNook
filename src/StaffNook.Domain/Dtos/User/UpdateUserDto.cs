using StaffNook.Domain.Dtos.Attachments;

namespace StaffNook.Domain.Dtos.User;

public class UpdateUserDto
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
    public FileDto Attachment { get; set; }
    
    /// <summary>
    /// Роль
    /// </summary>
    public Guid RoleId { get; set; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime DateOfBirth { get; set; }
    
    /// <summary>
    /// Дата приема
    /// </summary>
    public DateTime EmploymentDate { get; set; }
    
    /// <summary>
    /// Специальность
    /// </summary>
    public Guid? SpecialityId { get; set; }
}