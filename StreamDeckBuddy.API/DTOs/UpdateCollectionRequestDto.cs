using StreamDeckBuddy.Models;

namespace StreamDeckBuddy.API.DTOs;

public class UpdateCollectionRequestDto
{
    public UserIconCollectionId Id { get; set; }
    public string? Name { get; set; }
    public List<StylizedIconDto> Icons { get; set; } = [];
    
    public class StylizedIconDto
    {
        public UserIconId Id { get; set; }
        public IconId OriginalIconId { get; set; }
        public string? Label { get; set; }
        public bool? LabelVisible { get; set; } = true;
        public string? LabelColor { get; set; }
        public string? LabelTypeface { get; set; }
        public string? GlyphColor { get; set; }
        public string? BackgroundColor { get; set; }
        public double IconScale { get; set; } = 1.0;
        public int ImgX { get; set; }
        public int ImgY { get; set; }
        public int LabelX { get; set; }
        public int LabelY { get; set; }
        public string? PngData { get; set; }
    }
    
    public UserIconCollection ToDomain()
    {
        return new UserIconCollection
        {
            Id = Id,
            Name = Name,
            Icons = Icons.Select(i => new UserIcon
            {
                Id = i.Id,
                IconId = i.OriginalIconId,
                Label = i.Label,
                LabelVisible = i.LabelVisible ?? true,
                LabelColor = i.LabelColor ?? throw new ArgumentNullException(nameof(i.LabelColor)),
                LabelTypeface = i.LabelTypeface ?? throw new ArgumentNullException(nameof(i.LabelTypeface)),
                GlyphColor = i.GlyphColor ?? throw new ArgumentNullException(nameof(i.GlyphColor)),
                BackgroundColor = i.BackgroundColor ?? throw new ArgumentNullException(nameof(i.BackgroundColor)),
                IconScale = i.IconScale,
                ImgX = i.ImgX,
                ImgY = i.ImgY,
                LabelX = i.LabelX,
                LabelY = i.LabelY,
                PngData = i.PngData ?? throw new ArgumentNullException(nameof(i.PngData))
            }).ToList()
        };
    }
}