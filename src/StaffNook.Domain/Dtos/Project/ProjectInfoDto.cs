namespace StaffNook.Domain.Dtos.Project;

public class ProjectInfoDto : BaseInfoDto
{
    /// <summary>
    /// Название проекта
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Руководитель проекта
    /// </summary>
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    
    /// <summary>
    /// Клиент (заказчик)
    /// </summary>
    public Guid ClientId { get; set; }
    public string ClientName { get; set; }
    
    /// <summary>
    /// Дата начала
    /// </summary>
    public DateOnly StartDate { get; set; }
    
    /// <summary>
    /// Дата окончания
    /// </summary>
    public DateOnly EndDate { get; set; }
    
    /// <summary>
    /// Тип проекта
    /// </summary>
    public Guid ProjectTypeId { get; set; }
    public string ProjectTypeName { get; set; }
    
    /// <summary>
    /// Контакты проекта
    /// </summary>
    public IEnumerable<ProjectContactDto> ProjectContacts { get; set; }
    
    /// <summary>
    /// Роли на проекте
    /// </summary>
    public IEnumerable<ProjectRoleDto> ProjectRoles { get; set; }
    
    /// <summary>
    /// Сотрудники на проекте
    /// </summary>
    public IEnumerable<ProjectEmployeeDto> ProjectEmployees { get; set; }
}