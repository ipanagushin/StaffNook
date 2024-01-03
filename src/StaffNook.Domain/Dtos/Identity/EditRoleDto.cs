namespace StaffNook.Domain.Dtos.Identity;

public class EditRoleDto
{
    /// <summary>
    /// Имя роли
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Список клеймов роли
    /// </summary>
    public IEnumerable<string> Claims { get; set; }
}