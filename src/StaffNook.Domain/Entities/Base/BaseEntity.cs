namespace StaffNook.Domain.Entities.Base;

public class BaseEntity
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    public bool IsActive { get; set; }

    public DateTime DateCreated { get; set; }

    public Guid? CreatorId { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DateDelete { get; set; }

    public DateTime DateUpdated { get; set; }
}