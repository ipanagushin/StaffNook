using StaffNook.Domain.Entities.Base;
using StaffNook.Domain.Entities.Project;

namespace StaffNook.Domain.Entities.Employee;

/// <summary>
/// Отчет о затраченном времени сотрудником
/// </summary>
public class EmployeeWorkingTimeEntity : BaseEntity
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid EmployeeId { get; set; }
    public virtual EmployeeEntity Employee { get; set; }
    
    /// <summary>
    /// Проект
    /// </summary>
    public Guid ProjectId { get; set; }
    public virtual ProjectEntity Project { get; set; }

    /// <summary>
    /// Затраченное время
    /// </summary>
    public double Time { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }
}