using System.Text.Json.Serialization;
using StreamDeckBuddy.Models.Converters;

namespace StreamDeckBuddy.Models;

public class Collection
{
    public CollectionId Id { get; set; }
    public string? Name { get; set; }
    public List<StylizedIcon> Icons { get; set; } = [];
}

[JsonConverter(typeof(CollectionIdJsonConverter))]
public struct CollectionId(Guid value) : IEquatable<CollectionId>
{
    public Guid Value { get; } = value;

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(CollectionId iconId) => iconId.Value;

    public static implicit operator CollectionId(Guid value) => new(value);

    public override bool Equals(object? obj) => obj is CollectionId other && Equals(other);

    public bool Equals(CollectionId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(CollectionId left, CollectionId right) => left.Equals(right);

    public static bool operator !=(CollectionId left, CollectionId right) => !(left == right);
}