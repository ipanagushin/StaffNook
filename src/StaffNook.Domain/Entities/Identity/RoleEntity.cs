using StaffNook.Domain.Entities.Base;

namespace StaffNook.Domain.Entities.Identity;

public class RoleEntity : BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<ClaimsRolesEntity> RoleClaims { get; set; }
}