using StaffNook.Domain.Dtos.Attachments;

namespace StaffNook.Domain.Dtos.User;

public class ShortUserInfoDto
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public Guid RoleId { get; set; }
    public string Email { get; set; }
    
    /// <summary>
    /// Вложение (фото аватарка)
    /// </summary>
    public Guid AttachmentId { get; set; }
    
    public FileDto Attachment { get; set; }
}