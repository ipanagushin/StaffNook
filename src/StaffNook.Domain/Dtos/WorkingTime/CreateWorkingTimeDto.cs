namespace StaffNook.Domain.Dtos.WorkingTime;

public class CreateWorkingTimeDto
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