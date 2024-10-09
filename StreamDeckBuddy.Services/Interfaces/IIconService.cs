using StreamDeckBuddy.Models;

namespace StreamDeckBuddy.Services.Interfaces;

public interface IIconService
{
    ICollection<Icon> GetIcons();
    Icon? GetIconById(IconId id);
}