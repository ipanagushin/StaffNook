using StaffNook.Domain.Entities.Base;

namespace StaffNook.Domain.Entities.News;

public class NewsEntity : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
}