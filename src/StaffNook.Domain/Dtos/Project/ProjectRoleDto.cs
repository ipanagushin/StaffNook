namespace StaffNook.Domain.Dtos.Project;

public class ProjectRoleDto
{
    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Ставка за час
    /// </summary>
    public double HourlyFee { get; set; }
    
    /// <summary>
    /// Проект
    /// </summary>
    public Guid? ProjectId { get; set; }
}