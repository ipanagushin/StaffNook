using StaffNook.Domain.Dtos.Attachments;

namespace StaffNook.Domain.Dtos.User;

public class ShortUserInfoDto
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// ФИО
    /// </summary>
    public string FullName { get; set; }
    
    /// <summary>
    /// Специальность
    /// </summary>
    public string SpecialityName { get; set; }
    
    /// <summary>
    /// Дата приема
    /// </summary>
    public DateTime EmploymentDate { get; set; }
    
    /// <summary>
    /// Аватар
    /// </summary>
    public FileDto Attachment { get; set; }
}