namespace StreamDeckBuddy.Models;

public class Collection
{
    public Guid Id { get; set; }
    public required string Name { get; set; }                // Name of the collection
    public List<Icon> Icons { get; set; } = [];  // List of icons in the collection
}