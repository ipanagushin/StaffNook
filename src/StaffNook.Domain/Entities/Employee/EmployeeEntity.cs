#nullable disable
using System.Collections;
using StaffNook.Domain.Entities.Base;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Entities.Project;

namespace StaffNook.Domain.Entities.Employee;

public class EmployeeEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; }

    /// <summary>
    /// Имя сотрудника
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Отчество сотрудника
    /// </summary>
    public string Patronymic { get; set; }

    /// <summary>
    /// Фамилия сотрудника
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    // TODO:: AddFileStorage
    // public string PhotoKey { get; set; }

    /// <summary>
    /// Ставка за час
    /// </summary>
    public double HourlyFee { get; set; }
    
    /// <summary>
    /// Отчеты о затраченном времени
    /// </summary>
    public virtual IEnumerable<EmployeeWorkingTimeEntity> EmployeeWorkingTimes { get; set; }
    
    /// <summary>
    /// Проекты которыми управляет сотрудник
    /// </summary>
    // public virtual IEnumerable<ProjectEntity> ManagedProjects { get; set; }
}