namespace StaffNook.Domain.Dtos.Project;

public class ProjectEmployeeDto
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Роль на проекте
    /// </summary>
    public Guid ProjectRoleId { get; set; }
    
    /// <summary>
    /// Проект
    /// </summary>
    public Guid ProjectId { get; set; }
}