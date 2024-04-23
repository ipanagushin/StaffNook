namespace StaffNook.Domain.Dtos.Client;

public class ClientInfoDto : BaseInfoDto
{
    /// <summary>
    /// Полное название
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Короткое название
    /// </summary>
    public string ShortName { get; set; }
}