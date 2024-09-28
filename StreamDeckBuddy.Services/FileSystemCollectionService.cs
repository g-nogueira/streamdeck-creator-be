using StreamDeckBuddy.Models.Converters;

namespace StreamDeckBuddy.Services;

using Microsoft.Extensions.Logging;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using StreamDeckBuddy.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class FileSystemCollectionService : ICollectionService
{
    private readonly List<Collection> _collections = [];
    private readonly string _jsonFilePath;
    private readonly ILogger<FileSystemCollectionService> _logger;
    public FileSystemCollectionService(IConfiguration configuration, ILogger<FileSystemCollectionService> logger)
    {
        var collectionsPath = configuration["DataPaths:CollectionsFilePath"] ?? throw new InvalidOperationException("Collections file path not configured.");
        _jsonFilePath = Path.Combine(collectionsPath, "collections.json");
        _logger = logger;

        if (File.Exists(_jsonFilePath))
        {
            LoadCollectionsFromJson();
        }
        else
        {
            SaveCollectionsToJson();
        }
    }

    public List<Collection> GetCollections() => _collections;

    [Pure]
    public Collection? GetCollectionById(CollectionId id) => _collections.FirstOrDefault(c => c.Id == id);

    public CollectionId AddCollection(Collection collection)
    {
        collection.Id = Guid.NewGuid();
        _collections.Add(collection);
        SaveCollectionsToJson();
        
        return collection.Id;
    }

    public void UpdateCollection(CollectionId id, Collection updatedCollection)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == id);
        if (collection == null) return;

        collection.Name = updatedCollection.Name;
        collection.Icons = updatedCollection.Icons;
        SaveCollectionsToJson();
    }

    public void DeleteCollection(CollectionId id)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == id);
        if (collection == null) return;

        _collections.Remove(collection);
        SaveCollectionsToJson();
    }

    public void AddIconToCollection(CollectionId collectionId, StylizedIcon icon)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == collectionId);
        if (collection == null) return;
        
        var existingIcon = collection.Icons.FirstOrDefault(i => i.Id == icon.Id);
        
        // Upsert the icon
        if (icon.Id != new StylizedIconId(Guid.Empty) && existingIcon != null)
        {
            existingIcon.LabelText = icon.LabelText;
            existingIcon.LabelVisible = icon.LabelVisible;
            existingIcon.LabelColor = icon.LabelColor;
            existingIcon.LabelTypeface = icon.LabelTypeface;
            existingIcon.GlyphColor = icon.GlyphColor;
            existingIcon.BackgroundColor = icon.BackgroundColor;
            
            collection.Icons.RemoveAt(collection.Icons.IndexOf(existingIcon));
            collection.Icons.Add(existingIcon);
        } else {
            icon.Id = new StylizedIconId(Guid.NewGuid());
            collection.Icons.Add(icon);
        }

        SaveCollectionsToJson();
    }
    
    private void SaveCollectionsToJson()
    {
        try
        {
            var directory = Path.GetDirectoryName(_jsonFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(_collections);
            File.WriteAllText(_jsonFilePath, json);
            _logger.LogInformation("Successfully saved collections to {FilePath}", _jsonFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving collections to JSON file.");
        }
    }

    private void LoadCollectionsFromJson()
    {
        try
        {
            var json = File.ReadAllText(_jsonFilePath);
            var collections = JsonSerializer.Deserialize<List<Collection>>(json);
            if (collections == null) return;

            _collections.AddRange(collections);
            _logger.LogInformation("Successfully loaded collections from {FilePath}", _jsonFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while loading collections from JSON file.");
        }
    }
}
