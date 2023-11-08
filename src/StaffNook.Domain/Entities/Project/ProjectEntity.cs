using StaffNook.Domain.Entities.Base;
using StaffNook.Domain.Entities.Client;
using StaffNook.Domain.Entities.Employee;
using StaffNook.Domain.Entities.Reference;

namespace StaffNook.Domain.Entities.Project;

/// <summary>
/// Проект
/// </summary>
public class ProjectEntity : BaseEntity
{
    /// <summary>
    /// Название проекта
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Руководитель проекта
    /// </summary>
    public Guid ManagerId { get; set; }
    public virtual EmployeeEntity Manager { get; set; }
    
    /// <summary>
    /// Клиент (заказчик)
    /// </summary>
    public Guid ClientId { get; set; }
    public virtual ClientEntity Client { get; set; }
    
    /// <summary>
    /// Дата начала
    /// </summary>
    public DateOnly StartDate { get; set; }
    
    /// <summary>
    /// Дата окончания
    /// </summary>
    public DateOnly EndDateDate { get; set; }
    
    /// <summary>
    /// Тип проекта
    /// </summary>
    public Guid ProjectTypeId { get; set; }
    public virtual ProjectTypeEntity ProjectType { get; set; }
    
    /// <summary>
    /// Сотрудники проекта
    /// </summary>
    public virtual ICollection<ProjectEmployeeEntity> ProjectEmployees { get; set; }
    
    /// <summary>
    /// Контакты проекта
    /// </summary>
    public virtual ICollection<ProjectContactsEntity> ProjectContacts { get; set; }
}