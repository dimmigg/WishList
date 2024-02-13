using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Storage.Storages.Users;

public class UserStorage(
    WishListDbContext dbContext,
    IMapper mapper) : IUserStorage
{
    public async Task<User> AddUser(User user, CancellationToken cancellationToken)
    {
        var localUser = mapper.Map<TelegramUser>(user);
        await dbContext.Users.AddAsync(localUser, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Users
            .Where(u => u.Id == user.Id)
            .ProjectTo<User>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
    
    public async Task<User?> GetUser(long id, CancellationToken cancellationToken)
    {
        var query = dbContext.Users.Where(u => u.Id == id);
        var user = await query.FirstOrDefaultAsync(cancellationToken);
        return mapper.Map<User>(user);
    }
    
    public async Task UpdateUser(User user, CancellationToken cancellationToken)
    {
        var localUser = mapper.Map<TelegramUser>(user);
        var existingUser = await dbContext.Users.FindAsync(new object?[] { user.Id }, cancellationToken: cancellationToken);

        if (existingUser != null)
        {
            dbContext.Entry(existingUser).CurrentValues.SetValues(localUser);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}