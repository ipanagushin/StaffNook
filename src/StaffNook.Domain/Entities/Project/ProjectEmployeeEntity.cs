using StaffNook.Domain.Entities.Base;
using StaffNook.Domain.Entities.Employee;

namespace StaffNook.Domain.Entities.Project;

/// <summary>
/// Сотрудники привязанные к проекту
/// </summary>
public class ProjectEmployeeEntity : BaseEntity
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid EmployeeId { get; set; }
    public virtual EmployeeEntity Employee { get; set; }
    
    /// <summary>
    /// Роль на проекте
    /// </summary>
    public Guid ProjectRoleId { get; set; }
    public virtual ProjectRoleEntity ProjectRole { get; set; }
    
    /// <summary>
    /// Проект
    /// </summary>
    public Guid ProjectId { get; set; }
    public virtual ProjectEntity Project { get; set; }
}