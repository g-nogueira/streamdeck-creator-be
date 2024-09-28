namespace StreamDeckBuddy.Models.Converters;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class StylizedIconIdJsonConverter : JsonConverter<StylizedIconId>
{
    public override StylizedIconId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetGuid();
        return new StylizedIconId(value);
    }

    public override void Write(Utf8JsonWriter writer, StylizedIconId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}