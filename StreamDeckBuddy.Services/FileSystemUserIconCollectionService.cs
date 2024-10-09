using System.Text.Json;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StreamDeckBuddy.Models;

namespace StreamDeckBuddy.Services;

public class FileSystemUserIconCollectionService : IUserIconCollectionService
{
    private readonly List<UserIconCollection> _collections = [];
    private readonly string _jsonFilePath;
    private readonly ILogger<FileSystemUserIconCollectionService> _logger;

    public FileSystemUserIconCollectionService(IConfiguration configuration, ILogger<FileSystemUserIconCollectionService> logger)
    {
        var collectionsPath = configuration["DataPaths:CollectionsFilePath"] ??
                              throw new InvalidOperationException("Collections file path not configured.");
        _jsonFilePath = Path.Combine(collectionsPath, "collections.json");
        _logger = logger;

        if (File.Exists(_jsonFilePath))
            LoadCollectionsFromJson();
        else
            SaveCollectionsToJson();
    }

    public IEnumerable<UserIconCollection> GetList() => _collections;

    [Pure]
    public UserIconCollection? GetById(UserIconCollectionId id)
    {
        return _collections.FirstOrDefault(c => c.Id == id);
    }

    public UserIconCollectionId Add(UserIconCollection userIconCollection)
    {
        userIconCollection.Id = Guid.NewGuid();
        userIconCollection.Icons = userIconCollection.Icons.Select(icon =>
        {
            if (icon.Id == UserIconId.Empty)
                icon.Id = UserIconId.New();
            return icon;
        }).ToList();

        _collections.Add(userIconCollection);
        SaveCollectionsToJson();

        return userIconCollection.Id;
    }

    public void UpdateCollection(UserIconCollectionId id, UserIconCollection updatedUserIconCollection)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == id);
        
        // TODO: Throw CollectionNotFoundException if collection is null
        if (collection == null)
            throw new CollectionNotFoundException(id);

        collection.Name = updatedUserIconCollection.Name;
        collection.Icons = updatedUserIconCollection.Icons.Select(icon =>
        {
            if (icon.Id == UserIconId.Empty)
                icon.Id = UserIconId.New();
            return icon;
        }).ToList();

        SaveCollectionsToJson();
    }

    public void DeleteCollection(UserIconCollectionId id)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == id);
        if (collection == null)
            throw new CollectionNotFoundException(id);

        _collections.Remove(collection);
        SaveCollectionsToJson();
    }

    public void AddIconToCollection(UserIconCollectionId userIconCollectionId, UserIcon icon)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == userIconCollectionId);
        if (collection == null)
            throw new CollectionNotFoundException(userIconCollectionId);

        var existingIcon = collection.Icons.FirstOrDefault(i => i.Id == icon.Id);

        // Upsert the icon
        if (icon.Id != UserIconId.Empty && existingIcon != null)
        {
            existingIcon.Label = icon.Label;
            existingIcon.LabelVisible = icon.LabelVisible;
            existingIcon.LabelColor = icon.LabelColor;
            existingIcon.LabelTypeface = icon.LabelTypeface;
            existingIcon.GlyphColor = icon.GlyphColor;
            existingIcon.BackgroundColor = icon.BackgroundColor;
            existingIcon.IconScale = icon.IconScale;
            existingIcon.ImgX = icon.ImgX;
            existingIcon.ImgY = icon.ImgY;
            existingIcon.LabelX = icon.LabelX;
            existingIcon.LabelY = icon.LabelY;
            existingIcon.PngData = icon.PngData;

            collection.Icons.RemoveAt(collection.Icons.IndexOf(existingIcon));
            collection.Icons.Add(existingIcon);
        }
        else
        {
            icon.Id = UserIconId.New();
            collection.Icons.Add(icon);
        }

        SaveCollectionsToJson();
    }

    private void SaveCollectionsToJson()
    {
        try
        {
            var directory = Path.GetDirectoryName(_jsonFilePath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

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
            var collections = JsonSerializer.Deserialize<List<UserIconCollection>>(json);
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

public class CollectionNotFoundException(UserIconCollectionId id) : Exception($"Collection with ID {id} not found.");