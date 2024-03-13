namespace StaffNook.Domain.Dtos.Identity;

public class CurrentUserResponseDto
{
    public Guid Id { get; set; }
        
    public Guid RoleId { get; set; }
    
    public bool IsAdmin { get; set; }
        
    public string Login { get; set; }
    
    public string Email { get; set; }
    
    public string FullName { get; set; }
    
    public string AvatarLink { get; set; }
    // ToDo:: add avatar attachment
    // public Guid AvatarId { get; set; }
}