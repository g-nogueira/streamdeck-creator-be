namespace StreamDeckBuddy.Services;

using StreamDeckBuddy.Models;
using System.Collections.Generic;
using System.Linq;

public class FileSystemCollectionService : ICollectionService
{
    private readonly List<Collection> _collections = new();

    public List<Collection> GetCollections() => _collections;

    public Collection GetCollectionById(Guid id) => _collections.FirstOrDefault(c => c.Id == id);

    public void AddCollection(Collection collection)
    {
        collection.Id = Guid.NewGuid();
        _collections.Add(collection);
    }

    public void UpdateCollection(Guid id, Collection updatedCollection)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == id);
        if (collection != null)
        {
            collection.Name = updatedCollection.Name;
            collection.Icons = updatedCollection.Icons;
        }
    }

    public void DeleteCollection(Guid id)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == id);
        if (collection != null)
        {
            _collections.Remove(collection);
        }
    }
}