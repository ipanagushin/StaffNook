using System.Net.Mail;

namespace StaffNook.Infrastructure.Common;

public static class StringValidationHelper
{
    public static bool IsValidNumber(string phoneNumber)
    {
        return phoneNumber.StartsWith("+7") && phoneNumber.Length == 12 && phoneNumber.Skip(1).All(char.IsDigit);
    }

    public static bool IsValidEmailAddress(string emailAddress)
    {
        try
        {
            // Пытаемся создать объект MailAddress, это вызовет исключение, если email недопустим
            _ = new MailAddress(emailAddress);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}