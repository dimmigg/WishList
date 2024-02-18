using Microsoft.EntityFrameworkCore;

namespace WishList.Storage.Storages.Presents;

public class PresentStorage(
    WishListDbContext dbContext)
    : IPresentStorage
{
    public async Task<Entities.Present[]> GetPresents(int wishListId, CancellationToken cancellationToken) =>
        await dbContext.Presents
            .Where(wl => wl.WishListId == wishListId)
            .ToArrayAsync(cancellationToken);
}