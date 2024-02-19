using Microsoft.EntityFrameworkCore;
using WishList.Storage.Exceptions;

namespace WishList.Storage.Storages.WishLists;

public class WishListStorage(
    WishListDbContext dbContext) : IWishListStorage
{
    public async Task<Entities.WishList> AddWishList(string name, long userId, CancellationToken cancellationToken)
    {
        var wishList = new Entities.WishList()
        {
            Name = name.Trim(),
            AuthorId = userId
        };
        await dbContext.WishLists.AddAsync(wishList, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return wishList;
    }

    public async Task<Entities.WishList?> GetWishList(int id, CancellationToken cancellationToken) =>
        await dbContext.WishLists
            .Where(wl => wl.Id == id)
            .Include(wl => wl.Presents)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Entities.WishList[]> GetWishLists(long userId, CancellationToken cancellationToken) =>
        await dbContext.WishLists
            .Where(wl => wl.AuthorId == userId)
            .Include(wl => wl.Presents)
            .ToArrayAsync(cancellationToken);

    public async Task<Entities.WishList> UpdateNameWishList(int wishListId, string name, long authorId, CancellationToken cancellationToken)
    {
        var existingWishList = await GetWishList(wishListId, cancellationToken: cancellationToken) ??
                               await AddWishList(name, authorId, cancellationToken);

        existingWishList.Name = name;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return existingWishList;
    }
    
    public async Task<Entities.WishList> UpdatePrivateWishList(int wishListId, bool isPrivate, CancellationToken cancellationToken)
    {
        var existingWishList = await GetWishList(wishListId, cancellationToken: cancellationToken);
        if(existingWishList == null) throw new StorageException("WishList was not found");
        
        existingWishList.IsPrivate = isPrivate;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return existingWishList;
    }
}