using System.ComponentModel.DataAnnotations.Schema;
using StaffNook.Domain.Entities.Identity;

namespace StaffNook.Domain.Entities.Base;

public class BaseEntity
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatorId { get; set; }

    public bool IsArchived { get; set; }

    public DateTime? UpdatedAt { get; set; }
}