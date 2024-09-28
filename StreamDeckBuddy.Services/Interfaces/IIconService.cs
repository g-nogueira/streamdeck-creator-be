using StreamDeckBuddy.Models;

namespace StreamDeckBuddy.Services;

public interface IIconService
{
    ICollection<Icon> GetIcons();
    Icon? GetIconById(IconId id);
    void AddIcon(Icon icon);
    void UpdateIcon(IconId id, Icon updatedIcon);
    void DeleteIcon(IconId id);
}