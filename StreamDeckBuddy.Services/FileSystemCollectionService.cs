namespace StreamDeckBuddy.Services;

using StreamDeckBuddy.Models;
using System.Collections.Generic;
using System.Linq;

public class FileSystemCollectionService : ICollectionService
{
    private readonly List<Collection> _collections = new();

    public List<Collection> GetCollections() => _collections;

    public Collection GetCollectionById(int id) => _collections.FirstOrDefault(c => c.Id == id);

    public void AddCollection(Collection collection)
    {
        collection.Id = _collections.Count > 0 ? _collections.Max(c => c.Id) + 1 : 1;
        _collections.Add(collection);
    }

    public void UpdateCollection(int id, Collection updatedCollection)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == id);
        if (collection != null)
        {
            collection.Name = updatedCollection.Name;
            collection.Icons = updatedCollection.Icons;
        }
    }

    public void DeleteCollection(int id)
    {
        var collection = _collections.FirstOrDefault(c => c.Id == id);
        if (collection != null)
        {
            _collections.Remove(collection);
        }
    }
}