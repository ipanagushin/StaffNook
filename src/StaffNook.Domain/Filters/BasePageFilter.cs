namespace StaffNook.Domain.Filters;

public class BasePageFilter
{
    public int PageSize { get; set; } = 10;
    
    public int PageNumber { get; set; } = 1;
}