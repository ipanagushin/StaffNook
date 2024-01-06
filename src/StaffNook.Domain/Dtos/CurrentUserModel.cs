namespace StaffNook.Domain.Dtos;

public class CurrentUserModel
{
    public Guid Id { get; set; }
        
    public Guid RoleId { get; set; }
    
    public bool IsAdmin { get; set; }
        
    public string Login { get; set; }
}