using System.Text.Json.Serialization;
using StreamDeckBuddy.Models.Converters;

namespace StreamDeckBuddy.Models;

public class UserIconCollection
{
    public UserIconCollectionId Id { get; set; }
    public string? Name { get; set; }
    public List<UserIcon> Icons { get; set; } = [];
}

[JsonConverter(typeof(UserIconCollectionIdJsonConverter))]
public readonly struct UserIconCollectionId(Guid value) : IEquatable<UserIconCollectionId>
{
    public Guid Value { get; } = value;
    public static UserIconCollectionId Empty { get; } = new(Guid.Empty);
    
    public static UserIconCollectionId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(UserIconCollectionId iconId) => iconId.Value;

    public static implicit operator UserIconCollectionId(Guid value) => new(value);

    public override bool Equals(object? obj) => obj is UserIconCollectionId other && Equals(other);

    public bool Equals(UserIconCollectionId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(UserIconCollectionId left, UserIconCollectionId right) => left.Equals(right);

    public static bool operator !=(UserIconCollectionId left, UserIconCollectionId right) => !(left == right);
}