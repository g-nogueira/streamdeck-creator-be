
namespace StreamDeckBuddy.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StreamDeckBuddy.Services.DTOs;
using StreamDeckBuddy.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class StreamDeckIconPackIconService : IIconService
{
    private readonly List<Icon> _icons = new();
    private readonly string _jsonFilePath;
    private readonly ILogger<StreamDeckIconPackIconService> _logger;

    public StreamDeckIconPackIconService(IConfiguration configuration, ILogger<StreamDeckIconPackIconService> logger)
    {
        _rootPath = Environment.ExpandEnvironmentVariables(configuration["DataPaths:IconsFilePath"] ??
                                                           throw new InvalidOperationException());
        _logger = logger;
        IndexIcons(_rootPath);
    }

    public List<Icon> GetIcons()
    {
        _logger.LogInformation("Returning {Count} icons", _icons.Count);
        return _icons;
    }

    public Icon GetIconById(Guid id) => _icons.FirstOrDefault(i => i.Id == id);

    public void AddIcon(Icon icon)
    {
        icon.Id = Guid.NewGuid();
        _icons.Add(icon);
    }

    public void UpdateIcon(Guid id, Icon updatedIcon)
    {
        var icon = _icons.FirstOrDefault(i => i.Id == id);
        if (icon != null)
        {
            icon.Glyph = updatedIcon.Glyph;
            icon.Label = updatedIcon.Label;
        }
    }

    public void DeleteIcon(Guid id)
    {
        var icon = _icons.FirstOrDefault(i => i.Id == id);
        if (icon != null)
        {
            _icons.Remove(icon);
        }
    }

    // public async Task IndexIconsAsync(string rootPath)
    // {
    //     _logger.LogInformation("Starting to index icons from {RootPath}", rootPath);
    //     try
    //     {
    //         var directories = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
    //         var tasks = directories.Select(async directory =>
    //         {
    //             var iconsFilePath = Path.Combine(directory, "icons.json");
    //             if (File.Exists(iconsFilePath))
    //             {
    //                 _logger.LogInformation("Processing icons file: {IconsFilePath}", iconsFilePath);
    //                 var json = await File.ReadAllTextAsync(iconsFilePath);
    //                 var icons = JsonSerializer.Deserialize<List<Icon>>(json);
    //                 if (icons != null)
    //                 {
    //                     lock (_icons)
    //                     {
    //                         _icons.AddRange(icons);
    //                     }
    //                     _logger.LogInformation("Added {Count} icons from {IconsFilePath}", icons.Count, iconsFilePath);
    //                 }
    //             }
    //         });
    //
    //         await Task.WhenAll(tasks);
    //         _logger.LogInformation("Completed indexing icons.");
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "An error occurred while indexing icons.");
    //     }
    // }
    //
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
                        Id = new Guid(),
                        Glyph = dto.name,
                        Label = dto.name,
                        FullPath = Path.Combine(directory, "icons", dto.path) // Set the complete file path
                    }).ToList();

                    _icons.AddRange(icons);
                }

                _logger.LogInformation("Added {Count} icons from {IconsFilePath}", _icons.Count, iconsFilePath);
            }

            _logger.LogInformation("Completed indexing icons.");
        }
    }
}