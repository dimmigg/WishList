using WishList.Storage.Entities;

namespace WishList.Storage.Storages.Presents;

public interface IPresentStorage
{
    Task<Present?[]> GetPresents(int wishListId, CancellationToken cancellationToken);

    Task<Present?> AddPresent(string name, int wishListId, CancellationToken cancellationToken);
    Task<Present?> GetPresent(int presentId, CancellationToken cancellationToken);
    Task UpdateName(string name, int presentId, CancellationToken cancellationToken);
    Task UpdateReference(string reference, int presentId, CancellationToken cancellationToken);
    Task UpdateComment(string comment, int presentId, CancellationToken cancellationToken);
    Task Delete(int presentId, CancellationToken cancellationToken);
    Task<Present[]> GetSubscribePresents(int wishListId, CancellationToken cancellationToken);
    Task Reserve(int presentId, long reservedUserId, CancellationToken cancellationToken);
    Task RemoveReserve(int presentId, CancellationToken cancellationToken);
}