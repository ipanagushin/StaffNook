using Newtonsoft.Json;

namespace StaffNook.Infrastructure.Converters
{
    public class CancellationTokenRequestConverter : JsonConverter<CancellationToken>
    {
        public override CancellationToken ReadJson(JsonReader reader, Type objectType, CancellationToken existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Возвращаем значение по умолчанию для CancellationToken
            return default;
        }

        public override void WriteJson(JsonWriter writer, CancellationToken value, JsonSerializer serializer)
        {
            writer.WriteNull();
        }
    }
}