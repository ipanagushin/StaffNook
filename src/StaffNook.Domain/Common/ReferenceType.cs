using System.Text.Json.Serialization;

namespace StaffNook.Domain.Common;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReferenceType
{
    Speciality = 0,
}