namespace StreamDeckBuddy.Services;

using StreamDeckBuddy.Models;
using System.Collections.Generic;

public interface ICollectionService
{
    List<Collection> GetCollections();
    Collection GetCollectionById(Guid id);
    void AddCollection(Collection collection);
    void UpdateCollection(Guid id, Collection updatedCollection);
    void DeleteCollection(Guid id);
    Collection? GetCollectionById(CollectionId id);
    CollectionId AddCollection(Collection collection);
    void UpdateCollection(CollectionId id, Collection updatedCollection);
    void DeleteCollection(CollectionId id);
}