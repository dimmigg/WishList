using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using WishList.Storage.Entities;
using WishList.Storage.Exceptions;

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
    
    public Task<TelegramUser?> GetUser(long id, CancellationToken cancellationToken) =>
        dbContext.Users
            .Where(u => u.Id == id)
            .Include(u => u.WishLists)
            .Include(u => u.SubscribeWishLists)
            .ThenInclude(swl => swl.Author)
            .FirstOrDefaultAsync(cancellationToken);
    
    public async Task<TelegramUser> UpdateUser(User user, CancellationToken cancellationToken)
    {
        var existingUser = await GetUser(user.Id, cancellationToken: cancellationToken);

        if (existingUser == null) return await AddUser(user, cancellationToken);

        existingUser.Username = user.Username;
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return existingUser;
    }

    public async Task<TelegramUser> UpdateLastCommandUser(long id, string? command, CancellationToken cancellationToken)
    {
        var existingUser = await  dbContext.Users
            .Where(u => u.Id == id)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser == null) throw new StorageException("User not found");

        existingUser.LastCommand = command;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
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
        var user = await  dbContext.Users
            .Where(u => u.Id == userId)
            .Include(u => u.SubscribeWishLists)
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);
        var wishList = await dbContext.WishLists
            .FirstOrDefaultAsync(w => w.Id == wishListId, cancellationToken);
        if (user != null && wishList != null)
        {
            user.SubscribeWishLists.Add(wishList);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}