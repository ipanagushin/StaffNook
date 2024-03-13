namespace StaffNook.Domain.Dtos;

public class AvailableValue
{
    public Guid Value { get; set; }
    public string Name { get; set; }

    public AvailableValue(Guid value, string name)
    {
        Value = value;
        Name = name;
    }
}