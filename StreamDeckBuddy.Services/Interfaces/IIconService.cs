namespace StreamDeckBuddy.Services;

using StreamDeckBuddy.Models;
using System.Collections.Generic;

public interface IIconService
{
    List<Icon> GetIcons();
    Icon GetIconById(Guid id);
    void AddIcon(Icon icon);
    void UpdateIcon(Guid id, Icon updatedIcon);
    void DeleteIcon(Guid id);
    void IndexIcons(string rootPath);
}