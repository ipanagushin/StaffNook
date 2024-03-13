namespace StaffNook.Domain.Filters;

public class UserPageFilter : BasePageFilter
{
    public Guid? SpecialityId { get; set; }
    
    public string FullName { get; set; } 
    public string PhoneNumber { get; set; } 
}