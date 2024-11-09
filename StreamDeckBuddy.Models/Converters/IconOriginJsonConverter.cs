using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamDeckBuddy.Models.Converters;

public class IconOriginJsonConverter : JsonConverter<IconOrigin>
{
    public override IconOrigin Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()!;
        return new IconOrigin(value);
    }

    public override void Write(Utf8JsonWriter writer, IconOrigin value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}