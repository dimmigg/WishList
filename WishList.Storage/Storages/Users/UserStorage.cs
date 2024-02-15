using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using WishList.Storage.CommandOptions;
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
            .Include(u => u.ReadWishLists)
            .Include(u => u.WriteWishLists)
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

    public async Task<TelegramUser> UpdateWayUser(long id, Command command, CommandStep commandStep, CancellationToken cancellationToken)
    {
        var existingUser = await GetUser(id, cancellationToken: cancellationToken);

        if (existingUser == null) throw new StorageException("User not found");

        existingUser.Command = command;
        existingUser.CommandStep = commandStep;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return existingUser;
    }
}