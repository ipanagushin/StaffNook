namespace StaffNook.Domain.Dtos;

public class PaginationResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public PageInfo PageInfo { get; set; }
}

public class PageInfo
{
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPageCount { get; set; }
}