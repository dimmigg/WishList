using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Storage.Storages.Users;

public class UserStorage(
    WishListDbContext dbContext,
    IMapper mapper) : IUserStorage
{
    public async Task<TelegramUser> AddUser(User user, CancellationToken cancellationToken)
    {
        var localUser = mapper.Map<TelegramUser>(user);
        await dbContext.Users.AddAsync(localUser, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return localUser;
    }

    public Task<TelegramUser?> GetUser(long id, CancellationToken cancellationToken)
    {
        var query = dbContext.Users
            .Where(u => u.Id == id);
        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TelegramUser> UpdateUser(User user, CancellationToken cancellationToken)
    {
        var existingUser = await dbContext.Users
            .Where(u => u.Id == user.Id)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser is null) return await AddUser(user, cancellationToken);

        existingUser.Username = user.Username;
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        await dbContext.SaveChangesAsync(cancellationToken);
        dbContext.Entry(existingUser).State = EntityState.Detached;
        return await GetMainUser(existingUser.Id, cancellationToken);
    }

    public async Task<TelegramUser> GetMainUser(long userId, CancellationToken cancellationToken)
    {
        var existingUser = await dbContext.Users
            .Where(u => u.Id == userId)
            .Include(u => u.WishLists)
            .Include(u => u.SubscribeWishLists)!
            .ThenInclude(swl => swl.Author)
            .FirstAsync(cancellationToken);
        return existingUser;
    }

    public async Task<TelegramUser[]?> FindUsers(string findText, CancellationToken cancellationToken)
    {
        long.TryParse(findText, out var id);
        return await dbContext.Users
            .Where(u => (u.Username != null && u.Username.ToLower().Contains(findText.ToLower())) || u.Id == id)
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddSubscribeWishList(long userId, int wishListId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Where(u => u.Id == userId)
            .Include(u => u.SubscribeWishLists)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);
        var wishList = await dbContext.WishLists
            .FirstOrDefaultAsync(w => w.Id == wishListId, cancellationToken);
        if (user is not null && wishList is not null)
        {
            user.SubscribeWishLists?.Add(wishList);
            await dbContext.SaveChangesAsync(cancellationToken);
            dbContext.Entry(user).State = EntityState.Detached;
        }
    }
}