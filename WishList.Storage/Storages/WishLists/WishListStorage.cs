using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WishList.Storage.Exceptions;

namespace WishList.Storage.Storages.WishLists;

public class WishListStorage(
    WishListDbContext dbContext,
    IMapper mapper) : IWishListStorage
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

    public async Task<Entities.WishList> EditNameWishList(string name, int wishListId, CancellationToken cancellationToken)
    {
        var wishList = await GetDbWishList(wishListId).FirstOrDefaultAsync(cancellationToken);
        if (wishList is null) throw new StorageException("Список не найден");
        wishList.Name = name;
        await dbContext.SaveChangesAsync(cancellationToken);

        return wishList;
    }

    public async Task<Entities.WishList?> GetWishList(int id, CancellationToken cancellationToken) =>
        await dbContext.WishLists
            .Where(wl => wl.Id == id)
            .Include(wl => wl.Presents)
            .FirstOrDefaultAsync(cancellationToken);

    private IQueryable<Entities.WishList> GetDbWishList(int id) =>
        dbContext.WishLists
            .Where(wl => wl.Id == id)
            .AsTracking();

    public async Task<Entities.WishList[]> GetWishLists(long userId, CancellationToken cancellationToken) =>
        await dbContext.WishLists
            .Where(wl => wl.AuthorId == userId)
            .Include(wl => wl.Presents)
            .ToArrayAsync(cancellationToken);

    public async Task<Entities.WishList> UpdateNameWishList(int wishListId, string name, long authorId, CancellationToken cancellationToken)
    {
        var existingWishList = await GetDbWishList(wishListId).FirstOrDefaultAsync(cancellationToken) ??
                               await AddWishList(name, authorId, cancellationToken);

        existingWishList.Name = name;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return existingWishList;
    }

    public async Task<IEnumerable<Entities.WishList>> GetSubscribeWishLists(long userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Where(u => u.Id == userId)
            .Include(u => u.SubscribeWishLists)
            .ThenInclude(w => w.Author)
            .Include(u => u.SubscribeWishLists)
            .ThenInclude(w => w.Presents)
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null) throw new StorageException("Пользователь не найден");
        return user.SubscribeWishLists;
    }

    public async Task UnsubscribeWishList(long userId, int wishListId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Where(u => u.Id == userId)
            .Include(u => u.SubscribeWishLists)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null) throw new StorageException("Пользователь не найден");

        var wishList = await GetDbWishList(wishListId).FirstOrDefaultAsync(cancellationToken);
        if(wishList is null) throw new StorageException("Список не найден");

        user.SubscribeWishLists.Remove(wishList);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task Delete(int wishListId, CancellationToken cancellationToken)
    {
        var wishList = await GetDbWishList(wishListId).FirstOrDefaultAsync(cancellationToken);
        if (wishList == null)
            throw new StorageException("Список не найден");
        dbContext.Remove(wishList);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Entities.WishList> UpdatePrivateWishList(int wishListId, bool isPrivate, CancellationToken cancellationToken)
    {
        var existingWishList = await GetDbWishList(wishListId).FirstOrDefaultAsync(cancellationToken);
        if(existingWishList is null) throw new StorageException("WishList was not found");
        
        existingWishList.IsPrivate = isPrivate;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return existingWishList;
    }
}