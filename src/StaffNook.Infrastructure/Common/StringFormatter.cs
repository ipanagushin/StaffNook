namespace StaffNook.Infrastructure.Common;

public static class StringFormatter
{
    public static string GetEntityName(this Type type)
    {
        return type.Name.Replace("Entity", "");
    }
}