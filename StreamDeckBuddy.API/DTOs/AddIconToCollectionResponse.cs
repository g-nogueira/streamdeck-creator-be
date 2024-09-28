using JetBrains.Annotations;

namespace StreamDeckBuddy.API.DTOs;

using StreamDeckBuddy.Models;

public abstract class AddIconToCollectionResponse
{
    public Guid? Id { get; set; }
    public required Guid IconId { get; set; }
    public string? LabelText { get; set; }
    public bool? LabelVisible { get; set; } = true;
    public required string LabelColor { get; set; }
    public required string LabelTypeface { get; set; }
    public required string GlyphColor { get; set; }
    public required string BackgroundColor { get; set; }
    
    [Pure]
    public StylizedIcon ToDomain(Icon icon)
    {
        var stylizedIcon = new StylizedIcon
        {
            Id = Id.HasValue
                ? new StylizedIconId(Id.Value)
                : new StylizedIconId(Guid.Empty),
            LabelText = LabelText,
            LabelVisible = LabelVisible ?? true,
            LabelColor = LabelColor,
            LabelTypeface = LabelTypeface,
            GlyphColor = GlyphColor,
            BackgroundColor = BackgroundColor,
            Label = icon.Label,
            FullPath = icon.FullPath
        };

        ((Icon)stylizedIcon).Id = icon.Id;
        
        return stylizedIcon;
    }
}