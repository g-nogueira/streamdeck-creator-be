using StreamDeckBuddy.Models;

namespace StreamDeckBuddy.API.DTOs;

public class IconDto
{
    public Guid Id { get; set; }
    public required string Label { get; set; }
    
    public static IconDto FromDomain(Icon icon)
    {
        return new IconDto
        {
            Id = icon.Id,
            Label = icon.Label
        };
    }
}