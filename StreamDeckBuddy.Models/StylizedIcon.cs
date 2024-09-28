using System.Text.Json.Serialization;
using StreamDeckBuddy.Models.Converters;

namespace StreamDeckBuddy.Models;

/// <summary>
/// An Icon with styles applied
/// </summary>
public class StylizedIcon : Icon
{
    // Styles include:
    // Label text
    // Label visible
    // Label color
    // Label typeface
    // Glyph color
    // Background color
    
    public new StylizedIconId Id { get; set; }
    public string? LabelText { get; set; }
    public bool LabelVisible { get; set; } = true;
    public required string LabelColor { get; set; }
    public required string LabelTypeface { get; set; }
    public required string GlyphColor { get; set; }
    public required string BackgroundColor { get; set; }
}

[JsonConverter(typeof(StylizedIconIdJsonConverter))]
public struct StylizedIconId(Guid value) : IEquatable<StylizedIconId>
{
    public Guid Value { get; } = value;

    public override string ToString() => Value.ToString();
    public static implicit operator Guid(StylizedIconId iconId) => iconId.Value;
    public static implicit operator StylizedIconId(Guid value) => new(value);

    public override bool Equals(object? obj) => obj is StylizedIconId other && Equals(other);

    public bool Equals(StylizedIconId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(StylizedIconId left, StylizedIconId right) => left.Equals(right);
    public static bool operator !=(StylizedIconId left, StylizedIconId right) => !(left == right);
}