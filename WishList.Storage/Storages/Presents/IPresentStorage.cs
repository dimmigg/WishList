namespace WishList.Storage.Storages.Presents;

public interface IPresentStorage
{
    Task<Entities.Present[]> GetPresents(int wishListId, CancellationToken cancellationToken);
}