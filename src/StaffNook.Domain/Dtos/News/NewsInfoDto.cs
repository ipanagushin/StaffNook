namespace StaffNook.Domain.Dtos.News;

public class NewsInfoDto : BaseInfoDto
{
    /// <summary>
    /// Заголовок
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }
}