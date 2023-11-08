using StaffNook.Domain.Entities.Base;
using StaffNook.Domain.Entities.Project;

namespace StaffNook.Domain.Entities.Client;

/// <summary>
/// Клиент (заказчик)
/// </summary>
public class ClientEntity : BaseEntity
{
    /// <summary>
    /// Полное название
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Короткое название
    /// </summary>
    public string ShortName { get; set; }
    
    /// <summary>
    /// Проекты с которыми связан клиент
    /// </summary>
    public virtual ICollection<ProjectEntity> Projects { get; set; }
}