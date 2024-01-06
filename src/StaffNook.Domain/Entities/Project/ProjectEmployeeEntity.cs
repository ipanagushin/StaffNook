using StaffNook.Domain.Entities.Base;
using StaffNook.Domain.Entities.Employee;
using StaffNook.Domain.Entities.Identity;

namespace StaffNook.Domain.Entities.Project;

/// <summary>
/// Сотрудники привязанные к проекту
/// </summary>
public class ProjectEmployeeEntity : BaseEntity
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; }
    
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