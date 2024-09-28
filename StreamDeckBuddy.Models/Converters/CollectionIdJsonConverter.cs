using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamDeckBuddy.Models.Converters;

public class CollectionIdJsonConverter : JsonConverter<CollectionId>
{
    public override CollectionId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetGuid();
        return new CollectionId(value);
    }

    public override void Write(Utf8JsonWriter writer, CollectionId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}