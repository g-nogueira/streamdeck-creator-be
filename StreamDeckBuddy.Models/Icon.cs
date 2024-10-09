﻿using System.Text.Json.Serialization;
using StreamDeckBuddy.Models.Converters;

namespace StreamDeckBuddy.Models;

public class Icon
{
    public IconId Id { get; set; }
    public required string Label { get; set; }
    public required string FullPath { get; set; }
}

[JsonConverter(typeof(IconIdJsonConverter))]
public struct IconId(Guid value) : IEquatable<IconId>
{
    public Guid Value { get; } = value;
    public static IconId Empty { get; } = new(Guid.Empty);
    
    public static IconId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(IconId iconId) => iconId.Value;

    public static implicit operator IconId(Guid value) => new(value);

    public override bool Equals(object? obj) => obj is IconId other && Equals(other);

    public bool Equals(IconId other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(IconId left, IconId right) => left.Equals(right);

    public static bool operator !=(IconId left, IconId right) => !(left == right);
}