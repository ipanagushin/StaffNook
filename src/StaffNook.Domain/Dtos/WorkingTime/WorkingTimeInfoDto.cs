namespace StaffNook.Domain.Dtos.WorkingTime;

public class WorkingTimeInfoDto
{
    /// <summary>
    /// Проект
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// Затраченное время
    /// </summary>
    public double Time { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }
}