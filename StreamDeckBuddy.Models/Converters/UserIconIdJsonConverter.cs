using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamDeckBuddy.Models.Converters;

public class UserIconIdJsonConverter : JsonConverter<UserIconId>
{
    public override UserIconId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetGuid();
        return new UserIconId(value);
    }

    public override void Write(Utf8JsonWriter writer, UserIconId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}