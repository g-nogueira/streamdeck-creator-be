using StreamDeckBuddy.Models;

namespace StreamDeckBuddy.Services;

public interface ICollectionService
{
    List<Collection> GetCollections();
    Collection? GetCollectionById(CollectionId id);
    CollectionId AddCollection(Collection collection);
    void UpdateCollection(CollectionId id, Collection updatedCollection);
    void DeleteCollection(CollectionId id);
    void AddIconToCollection(CollectionId collectionId, StylizedIcon icon);
}