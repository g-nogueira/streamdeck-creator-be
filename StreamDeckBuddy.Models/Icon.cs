namespace StreamDeckBuddy.Models;

public class Icon
{
    public Guid Id { get; set; }
    public string? Glyph { get; set; }        // The glyph or symbol representing the icon
    public required string Label { get; set; }        // Custom label for the icon
    public required string FullPath { get; set; }     // Complete file path for the icon
}