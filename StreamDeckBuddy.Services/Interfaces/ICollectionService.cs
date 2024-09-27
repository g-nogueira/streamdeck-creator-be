namespace StreamDeckBuddy.Services;

using StreamDeckBuddy.Models;
using System.Collections.Generic;

public interface ICollectionService
{
    List<Collection> GetCollections();
    Collection GetCollectionById(int id);
    void AddCollection(Collection collection);
    void UpdateCollection(int id, Collection updatedCollection);
    void DeleteCollection(int id);
}