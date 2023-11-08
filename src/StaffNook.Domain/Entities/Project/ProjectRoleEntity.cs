using StaffNook.Domain.Entities.Base;

namespace StaffNook.Domain.Entities.Project;

/// <summary>
/// Роли на проекте
/// </summary>
public class ProjectRoleEntity : BaseEntity
{
    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Ставка за час
    /// </summary>
    public double HourlyFee { get; set; }
    
    /// <summary>
    /// Проект
    /// </summary>
    public Guid ProjectId { get; set; }
    public virtual ProjectEntity Project { get; set; }
}