namespace StaffNook.Domain.Dtos;

public class BaseInfoDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreateDate { get; set; }
    
    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdateDate { get; set; }
    
    /// <summary>
    /// Архивная запись
    /// </summary>
    public bool IsArchived { get; set; }
}