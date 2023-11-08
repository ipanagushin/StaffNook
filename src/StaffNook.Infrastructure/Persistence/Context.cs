using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StaffNook.Domain.Common;
using StaffNook.Domain.Entities.Attachment;
using StaffNook.Domain.Entities.Client;
using StaffNook.Domain.Entities.Employee;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Entities.Project;
using StaffNook.Domain.Entities.Reference;

namespace StaffNook.Infrastructure.Persistence;

public class Context : IdentityDbContext<UserEntity, RoleEntity, Guid>
{
    public Context(DbContextOptions<Context> options) : base(options)
    { }

    DbSet<EmployeeEntity> Employees { get; set; }
    DbSet<EmployeeWorkingTimeEntity> EmployeeWorkingTimes { get; set; }
    public DbSet<AttachmentEntity> Attachments { get; set; }
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<ProjectTypeEntity> ProjectTypes { get; set; }
    public DbSet<ProjectRoleEntity> ProjectRoles { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ProjectEmployeeEntity> ProjectEmployees { get; set; }
    public DbSet<ProjectContactsEntity> ProjectContacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        // modelBuilder.ApplyConfiguration(new DefaultAdminUserConfiguration());
        // modelBuilder.ApplyConfiguration(new DefaultAdminUserRoleConfiguration());
        
        // Добавляем для того чтобы в БД значение enum хранилось строкой
        modelBuilder.Entity<AttachmentEntity>()
            .Property(x => x.Bucket)
            .HasConversion(new EnumToStringConverter<FileStorageBucket>());

        modelBuilder.Entity<EmployeeWorkingTimeEntity>()
            .HasIndex(x => new { x.Id, x.EmployeeId }).IsUnique();

    }

}