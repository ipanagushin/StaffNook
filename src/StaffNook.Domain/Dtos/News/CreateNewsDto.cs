namespace StaffNook.Domain.Dtos.News;

public class CreateNewsDto
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