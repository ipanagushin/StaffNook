using System.ComponentModel.DataAnnotations.Schema;
using StaffNook.Domain.Entities.Attachment;
using StaffNook.Domain.Entities.Base;
using StaffNook.Domain.Entities.Employee;
using StaffNook.Domain.Entities.Reference;

namespace StaffNook.Domain.Entities.Identity;

public class UserEntity : BaseEntity
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
    /// Хеш
    /// </summary>
    public string Hash { get; set; }
    
    /// <summary>
    /// Соль
    /// </summary>
    public string Salt { get; set; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime DateOfBirth { get; set; }
    
    /// <summary>
    /// Дата приема
    /// </summary>
    public DateTime EmploymentDate { get; set; }
    
    /// <summary>
    /// Вложение (фото пользователя)
    /// </summary>
    public Guid? AttachmentId { get; set; }
    public virtual AttachmentEntity Attachment { get; set; }
    
    /// <summary>
    /// Специальность
    /// </summary>
    public Guid? SpecialityId { get; set; }
    public virtual SpecialityEntity Speciality { get; set; }
    
    /// <summary>
    /// Ставка за час
    /// </summary>
    public double HourlyFee { get; set; }
    
    /// <summary>
    /// Отчеты о затраченном времени
    /// </summary>
    public virtual IEnumerable<WorkingTimeEntity> EmployeeWorkingTimes { get; set; }
    
    /// <summary>
    /// Роль
    /// </summary>
    public Guid RoleId { get; set; }
    public virtual RoleEntity Role { get; set; }
    
    [NotMapped]
    public string FullName => MiddleName != null ? $"{LastName} {FirstName} {MiddleName}" : $"{LastName} {FirstName}";
}