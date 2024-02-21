using Microsoft.EntityFrameworkCore;
using WishList.Storage.Entities;
using WishList.Storage.Exceptions;

namespace WishList.Storage.Storages.Presents;

public class PresentStorage(
    WishListDbContext dbContext)
    : IPresentStorage
{
    public async Task<Present?[]> GetPresents(int wishListId, CancellationToken cancellationToken) =>
        await dbContext.Presents
            .Where(wl => wl.WishListId == wishListId)
            .ToArrayAsync(cancellationToken);

    public async Task<Present?> AddPresent(string name, int wishListId, CancellationToken cancellationToken)
    {
        var present = new Present()
        {
            Name = name,
            WishListId = wishListId
        };
        await dbContext.Presents.AddAsync(present, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return present;
    }

    public async Task<Present?> GetPresent(int presentId, CancellationToken cancellationToken) =>
        await dbContext.Presents
            .Where(wl => wl.Id == presentId)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Present> UpdateName(string name, int presentId, CancellationToken cancellationToken)
    {

        var present = await GetPresent(presentId, cancellationToken);
        if (present == null)
            throw new StorageException("Запись не найдена");

        present.Name = name;
        await dbContext.SaveChangesAsync(cancellationToken);

        return present;
    }
    
    public async Task<Present> UpdateReference(string reference, int presentId, CancellationToken cancellationToken)
    {

        var present = await GetPresent(presentId, cancellationToken);
        if (present == null)
            throw new StorageException("Запись не найдена");

        present.Reference = reference;
        await dbContext.SaveChangesAsync(cancellationToken);

        return present;
    }
    
    public async Task<Present> UpdateComment(string comment, int presentId, CancellationToken cancellationToken)
    {

        var present = await GetPresent(presentId, cancellationToken);
        if (present == null)
            throw new StorageException("Запись не найдена");

        present.Comment = comment;
        await dbContext.SaveChangesAsync(cancellationToken);

        return present;
    }

    public async Task Delete(int presentId, CancellationToken cancellationToken)
    {
        var present = await GetPresent(presentId, cancellationToken);
        if (present == null)
            throw new StorageException("Запись не найдена");
        dbContext.Remove(present);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Present[]> GetSubscribePresents(int wishListId, CancellationToken cancellationToken) => 
        await dbContext.Presents.Where(p => p.WishListId == wishListId).ToArrayAsync(cancellationToken);
}