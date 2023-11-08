using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StaffNook.Domain.Common;
using StaffNook.Domain.Entities.Identity;

namespace StaffNook.Infrastructure.Persistence;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasData(
            new RoleEntity
            {
                Id = Guid.Parse(IdentifierConstants.UserRoleId),
                Name = IdentityRoles.User,
                NormalizedName = IdentityRoles.User.ToUpper()
            },
            new RoleEntity
            {
                Id = Guid.Parse(IdentifierConstants.AdminRoleId),
                Name = IdentityRoles.Administrator,
                NormalizedName = IdentityRoles.Administrator.ToUpper()
            });
    }
}