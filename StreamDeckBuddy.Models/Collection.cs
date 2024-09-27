namespace StreamDeckBuddy.Models;

public class Collection
{
    public int Id { get; set; }
    public string Name { get; set; }                // Name of the collection
    public List<Icon> Icons { get; set; } = new();  // List of icons in the collection
}