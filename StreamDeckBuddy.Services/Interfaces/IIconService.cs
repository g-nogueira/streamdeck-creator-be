namespace StreamDeckBuddy.Services;

using StreamDeckBuddy.Models;
using System.Collections.Generic;

public interface IIconService
{
    List<Icon> GetIcons();
    Icon GetIconById(int id);
    void AddIcon(Icon icon);
    void UpdateIcon(int id, Icon updatedIcon);
    void DeleteIcon(int id);
}