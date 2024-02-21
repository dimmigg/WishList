namespace WishList.Storage.Storages.WishLists;

public interface IWishListStorage
{
    Task<Entities.WishList> AddWishList(string name, long userId, CancellationToken cancellationToken);
    Task<Entities.WishList?> GetWishList(int id, CancellationToken cancellationToken);
    Task<Entities.WishList[]> GetWishLists(long userId, CancellationToken cancellationToken);
    Task<Entities.WishList> UpdateNameWishList(int wishListId, string name, long authorId, CancellationToken cancellationToken);
    Task<IEnumerable<Entities.WishList>> GetSubscribeWishLists(long userId, CancellationToken cancellationToken);
    Task UnsubscribeWishList(long userId, int wishListId, CancellationToken cancellationToken);
}