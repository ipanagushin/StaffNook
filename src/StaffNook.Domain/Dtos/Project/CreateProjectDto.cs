namespace StaffNook.Domain.Dtos.Project;

public class CreateProjectDto
{
    /// <summary>
    /// Название проекта
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Руководитель проекта
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Клиент (заказчик)
    /// </summary>
    public Guid ClientId { get; set; }
    
    /// <summary>
    /// Дата начала
    /// </summary>
    public DateOnly StartDate { get; set; }
    
    /// <summary>
    /// Дата окончания
    /// </summary>
    public DateOnly EndDateDate { get; set; }
    
    /// <summary>
    /// Тип проекта
    /// </summary>
    public Guid ProjectTypeId { get; set; }
    
    /// <summary>
    /// Контакты проекта
    /// </summary>
    public IEnumerable<ProjectContactDto> Contacts { get; set; }
    
    /// <summary>
    /// Роли на проекте
    /// </summary>
    public IEnumerable<ProjectRoleDto> Roles { get; set; }
}