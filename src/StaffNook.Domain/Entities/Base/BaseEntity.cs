namespace StaffNook.Domain.Entities.Base;

public class BaseEntity
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid? CreatorId { get; set; }

    public bool IsArchived { get; set; }

    public DateTime? UpdateDate { get; set; }
}