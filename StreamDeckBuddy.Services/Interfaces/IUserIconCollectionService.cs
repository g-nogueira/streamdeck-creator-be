using StreamDeckBuddy.Models;

namespace StreamDeckBuddy.Services;

public interface IUserIconCollectionService
{
    IEnumerable<UserIconCollection> GetList();
    UserIconCollection? GetById(UserIconCollectionId id);
    UserIconCollectionId Add(UserIconCollection userIconCollection);
    UserIconCollection UpdateCollection(UserIconCollectionId id, UserIconCollection updatedUserIconCollection);
    void DeleteCollection(UserIconCollectionId id);
    void AddIconToCollection(UserIconCollectionId userIconCollectionId, UserIcon icon);
}