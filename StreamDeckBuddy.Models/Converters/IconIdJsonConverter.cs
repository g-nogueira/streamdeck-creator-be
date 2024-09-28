namespace StreamDeckBuddy.Models.Converters;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class IconIdJsonConverter : JsonConverter<IconId>
{
    public override IconId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetGuid();
        return new IconId(value);
    }

    public override void Write(Utf8JsonWriter writer, IconId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}