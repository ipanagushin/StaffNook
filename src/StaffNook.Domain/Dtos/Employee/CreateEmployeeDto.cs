namespace StaffNook.Domain.Dtos.Employee;

public class CreateEmployeeDto
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Имя сотрудника
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Отчество сотрудника
    /// </summary>
    public string Patronymic { get; set; }

    /// <summary>
    /// Фамилия сотрудника
    /// </summary>
    public string LastName { get; set; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime DateOfBirth { get; set; }
    
    /// <summary>
    /// Ставка за час
    /// </summary>
    public double HourlyFee { get; set; }
}