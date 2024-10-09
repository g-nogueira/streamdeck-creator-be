using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamDeckBuddy.Models.Converters;

public class UserIconCollectionIdJsonConverter : JsonConverter<UserIconCollectionId>
{
    public override UserIconCollectionId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetGuid();
        return new UserIconCollectionId(value);
    }

    public override void Write(Utf8JsonWriter writer, UserIconCollectionId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}