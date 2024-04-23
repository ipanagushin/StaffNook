namespace StaffNook.Domain.Dtos.Project;

public class ProjectContactDto
{
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; }
    
    /// <summary>
    /// Email адрес
    /// </summary>
    public string EmailAddress { get; set; }
    
    /// <summary>
    /// Дополнительная информация
    /// </summary>
    public string AdditionalInformation { get; set; }
    
    /// <summary>
    /// Проект
    /// </summary>
    public Guid? ProjectId { get; set; }
}