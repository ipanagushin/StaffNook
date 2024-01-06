using StaffNook.Domain.Entities.Base;

namespace StaffNook.Domain.Entities.Identity;

// ToDo:: Добавить ограничение в БД на создание по RoleId-Claim
public class ClaimsRolesEntity : BaseEntity
{
    /// <summary>
    /// Роль
    /// </summary>
    public Guid RoleId { get; set; }
    public virtual RoleEntity Role { get; set; }
    
    /// <summary>
    /// Клейм
    /// </summary>
    public string Claim { get; set; }
}