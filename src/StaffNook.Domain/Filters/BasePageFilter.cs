namespace StaffNook.Domain.Filters;

public class BasePageFilter
{
    /// <summary>
    /// Размер страницы
    /// <value>10</value>
    /// </summary>
    public int PageSize { get; set; } = 10;
    
    /// <summary>
    /// Номер страницы
    /// <value>1</value>
    /// </summary>
    public int PageNumber { get; set; } = 1;
}