using System.Text.Json.Serialization;
using StreamDeckBuddy.Models.Converters;

namespace StreamDeckBuddy.Models;

/// <summary>
///     An Icon with styles applied
/// </summary>
public class UserIcon
{
    public UserIconId Id { get; set; }
    
    public IconId IconId { get; set; }
    public string? Label { get; set; }
    public bool LabelVisible { get; set; } = true;
    public required string LabelColor { get; set; }
    public required string LabelTypeface { get; set; }
    public required string GlyphColor { get; set; }
    public required string BackgroundColor { get; set; }
    
    public double IconScale { get; set; } = 1.0;
    public int ImgX { get; set; }
    public int ImgY { get; set; }
    public int LabelX { get; set; }
    public int LabelY { get; set; }
    
    public bool UseGradient { get; set; }
    public UserIconGradient? Gradient { get; set; }
    public required string PngData { get; set; }
    
    public required IconOrigin Origin { get; set; }
}

public class UserIconGradient
{
    public List<IconGradientStop> Stops { get; set; } = [];
    public required string Type { get; set; }
    public double Angle { get; set; }
    public required string CssStyle { get; set; }
}

public class IconGradientStop
{
    public double Position { get; set; }
    public required string Color { get; set; }
}

[JsonConverter(typeof(UserIconIdJsonConverter))]
public readonly struct UserIconId(Guid value) : IEquatable<UserIconId>
{
    public Guid Value { get; } = value;
    
    public static UserIconId Empty { get; } = new(Guid.Empty);
    
    public static UserIconId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(UserIconId iconId) => iconId.Value;

    public static implicit operator UserIconId(Guid value) => new(value);

    public override bool Equals(object? obj) => obj is UserIconId other && Equals(other);

    public bool Equals(UserIconId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(UserIconId left, UserIconId right) => left.Equals(right);

    public static bool operator !=(UserIconId left, UserIconId right) => !(left == right);
}