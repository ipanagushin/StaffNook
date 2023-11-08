using StaffNook.Domain.Entities.Base;

namespace StaffNook.Domain.Entities.Project;

public class ProjectContactsEntity : BaseEntity
{
    /// <summary>
    /// Проект
    /// </summary>
    public Guid ProjectId { get; set; }
    public virtual ProjectEntity Project { get; set; }
    
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; }
    
    /// <summary>
    /// Email адрес
    /// </summary>
    public string EmailAddress { get; set; }
    
    /// <summary>
    /// Дополнительная информация
    /// </summary>
    public string AdditionalInformation { get; set; }
}