using System.Text.Json;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StreamDeckBuddy.Models;
using StreamDeckBuddy.Services.DTOs;

namespace StreamDeckBuddy.Services;

public class StreamDeckIconPackIconService : IIconService
{
    private readonly List<Icon> _icons = [];
    private readonly string _jsonFilePath;
    private readonly ILogger<StreamDeckIconPackIconService> _logger;

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public StreamDeckIconPackIconService(IConfiguration configuration, ILogger<StreamDeckIconPackIconService> logger)
    {
        var rootPath = Environment.ExpandEnvironmentVariables(configuration["DataPaths:StreamDeckIconsPath"] ??
                                                              throw new InvalidOperationException());
        var indexFilePath = Environment.ExpandEnvironmentVariables(
            configuration["DataPaths:StreamDeckIconsIndexedFilePath"] ?? throw new InvalidOperationException());
        _jsonFilePath = Path.Combine(indexFilePath, "indexed_icons.json");
        _logger = logger;

        if (!File.Exists(_jsonFilePath))
        {
            IndexIcons(rootPath);
            SaveIndexedItemsToJson();
        }
        else
        {
            LoadIndexedItemsFromJson();
        }
    }

    [Pure]
    public ICollection<Icon> GetIcons()
    {
        _logger.LogInformation("Returning {Count} icons", _icons.Count);
        return _icons;
    }

    [Pure]
    public Icon? GetIconById(IconId id)
    {
        return _icons.FirstOrDefault(i => i.Id == id);
    }

    public void AddIcon(Icon icon)
    {
        icon.Id = Guid.NewGuid();
        _icons.Add(icon);
    }

    public void UpdateIcon(IconId id, Icon updatedIcon)
    {
        var icon = _icons.FirstOrDefault(i => i.Id == id);
        if (icon != null) icon.Label = updatedIcon.Label;
    }

    public void DeleteIcon(IconId id)
    {
        var icon = _icons.FirstOrDefault(i => i.Id == id);
        if (icon != null) _icons.Remove(icon);
    }

    public void IndexIcons(string rootPath)
    {
        var directories = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
        foreach (var directory in directories)
        {
            var iconsFilePath = Path.Combine(directory, "icons.json");
            _logger.LogInformation("Processing icons file: {IconsFilePath}", iconsFilePath);
            if (File.Exists(iconsFilePath))
            {
                var json = File.ReadAllText(iconsFilePath);
                var iconDtos = JsonSerializer.Deserialize<List<StreamDeckIconPackDto>>(json);
                if (iconDtos != null)
                {
                    var icons = iconDtos.Select(dto => new Icon
                    {
                        Id = Guid.NewGuid(),
                        Label = dto.name,
                        FullPath = Path.Combine(directory, "icons", dto.path)
                    }).ToList();

                    _icons.AddRange(icons);
                }

                _logger.LogInformation("Added {Count} icons from {IconsFilePath}", _icons.Count, iconsFilePath);
            }

            _logger.LogInformation("Completed indexing icons.");
        }
    }

    private void SaveIndexedItemsToJson()
    {
        try
        {
            var directory = Path.GetDirectoryName(_jsonFilePath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(_icons);
            File.WriteAllText(_jsonFilePath, json);
            _logger.LogInformation("Successfully saved indexed items to {FilePath}", _jsonFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving indexed items to JSON file.");
        }
    }

    private void LoadIndexedItemsFromJson()
    {
        try
        {
            var json = File.ReadAllText(_jsonFilePath);
            var icons = JsonSerializer.Deserialize<List<Icon>>(json);
            if (icons == null) return;

            _icons.AddRange(icons);
            _logger.LogInformation("Successfully loaded indexed items from {FilePath}", _jsonFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while loading indexed items from JSON file.");
        }
    }
}