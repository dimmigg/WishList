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
    
    private IQueryable<Present> GetDbPresent(int presentId) =>
        dbContext.Presents
            .Where(p => p.Id == presentId)
            .AsTracking();

    public Task UpdateName(string name, int presentId, CancellationToken cancellationToken) =>
        dbContext.Presents
            .Where(p => p.Id == presentId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Name, name), cancellationToken);
    
    public  Task UpdateReference(string reference, int presentId, CancellationToken cancellationToken) =>
        dbContext.Presents
            .Where(p => p.Id == presentId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Reference, reference), cancellationToken);
    
    public Task UpdateComment(string comment, int presentId, CancellationToken cancellationToken) =>
        dbContext.Presents
            .Where(p => p.Id == presentId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Comment, comment), cancellationToken);

    public Task Delete(int presentId, CancellationToken cancellationToken) =>
        dbContext.Presents
            .Where(p => p.Id == presentId)
            .ExecuteDeleteAsync(cancellationToken);

    public async Task<Present[]> GetSubscribePresents(int wishListId, CancellationToken cancellationToken) => 
        await dbContext.Presents.Where(p => p.WishListId == wishListId).ToArrayAsync(cancellationToken);

    public async Task Reserve(int presentId, long reservedUserId, CancellationToken cancellationToken)
    {
        var present = await GetDbPresent(presentId).FirstOrDefaultAsync(cancellationToken);
        if (present == null)
            throw new StorageException("Запись не найдена");
        present.ReserveForUserId = reservedUserId;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task RemoveReserve(int presentId, CancellationToken cancellationToken) =>
        dbContext.Presents
            .Where(p => p.Id == presentId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.ReserveForUserId, (Func<Present, long?>)null!), cancellationToken);
}