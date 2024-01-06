#nullable disable
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace StaffNook.Infrastructure.Converters
{
    /// <summary>
    /// Определяет, в каких полях хранится конфиденциальная информация, или не нужная для логов
    /// и затерает их в логах
    /// <remarks>
    /// Пока что только пароль пользователей и cancellationToken
    /// </remarks>
    /// </summary>
    public class ShouldSerializeContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (property.PropertyName.ToLower().Contains("password"))
            {
                property.Converter = new SecureStringConverter();    
            }
            
            return property;
        }
    }
}