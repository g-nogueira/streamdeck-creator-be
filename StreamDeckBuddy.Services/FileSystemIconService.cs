namespace StreamDeckBuddy.Services;

using StreamDeckBuddy.Models;
using System.Collections.Generic;
using System.Linq;

public class FileSystemIconService : IIconService
{
    private readonly List<Icon> _icons = new();

    public List<Icon> GetIcons() => _icons;

    public Icon GetIconById(int id) => _icons.FirstOrDefault(i => i.Id == id);

    public void AddIcon(Icon icon)
    {
        icon.Id = _icons.Count > 0 ? _icons.Max(i => i.Id) + 1 : 1;
        _icons.Add(icon);
    }

    public void UpdateIcon(int id, Icon updatedIcon)
    {
        var icon = _icons.FirstOrDefault(i => i.Id == id);
        if (icon != null)
        {
            icon.Glyph = updatedIcon.Glyph;
            icon.Label = updatedIcon.Label;
            icon.Color = updatedIcon.Color;
            icon.CollectionId = updatedIcon.CollectionId;
        }
    }

    public void DeleteIcon(int id)
    {
        var icon = _icons.FirstOrDefault(i => i.Id == id);
        if (icon != null)
        {
            _icons.Remove(icon);
        }
    }
}