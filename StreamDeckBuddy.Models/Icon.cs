namespace StreamDeckBuddy.Models;

public class Icon
{
    public int Id { get; set; }
    public string Glyph { get; set; }        // The glyph or symbol representing the icon
    public string Label { get; set; }        // Custom label for the icon
    public int CollectionId { get; set; }    // Reference to the collection it belongs to
    public string FullPath { get; set; }     // Complete file path for the icon
}