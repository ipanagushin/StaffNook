#nullable disable
using Newtonsoft.Json;

namespace StaffNook.Infrastructure.Converters
{
    public class SecureStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var password = value as string;
            if (password == null)
            {
                writer.WriteNull();
                return;
            }

            password = new string(password.ToCharArray().Select(chr =>
            {
                if (chr >= '0' && chr <= '9') return '8';
                if (chr >= 'a' && chr <= 'z') return 'x';
                if (chr >= 'A' && chr <= 'Z') return 'X';
                return chr;
            }).OrderBy(x => x).ToArray());
            
            writer.WriteValue(password);
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // так и нужно оставить
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}