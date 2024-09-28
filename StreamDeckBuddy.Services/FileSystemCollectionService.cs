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
    private readonly List<Collection> _collections = new();

    public List<Collection> GetCollections() => _collections;

    [Pure]
    public Collection? GetCollectionById(CollectionId id) => _collections.FirstOrDefault(c => c.Id == id);

    public CollectionId AddCollection(Collection collection)
    {
        collection.Id = Guid.NewGuid();
        _collections.Add(collection);
    }

    public void UpdateCollection(CollectionId id, Collection updatedCollection)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == id);
        if (collection != null)
        {
            collection.Name = updatedCollection.Name;
            collection.Icons = updatedCollection.Icons;
        }
    }

    public void DeleteCollection(CollectionId id)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == id);
        if (collection != null)
        {
            _collections.Remove(collection);
        }
    }
}