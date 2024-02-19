using WishList.Storage.Entities;

namespace WishList.Storage.Storages.Presents;

public interface IPresentStorage
{
    Task<Present?[]> GetPresents(int wishListId, CancellationToken cancellationToken);

    Task<Present?> AddPresent(string name, int wishListId, CancellationToken cancellationToken);
    Task<Present?> GetPresent(int presentId, CancellationToken cancellationToken);
    Task<Present> UpdateName(string name, int presentId, CancellationToken cancellationToken);
    Task<Present> UpdateReference(string reference, int presentId, CancellationToken cancellationToken);
    Task<Present> UpdateComment(string comment, int presentId, CancellationToken cancellationToken);
    Task Delete(int presentId, CancellationToken cancellationToken);
}