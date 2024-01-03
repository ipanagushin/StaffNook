namespace StaffNook.Domain.Dtos.Identity;

public class RoleInfoDto : BaseInfoDto
{
    /// <summary>
    /// Имя роли
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Список клеймов роли
    /// </summary>
    public string[] Claims { get; set; }
}