using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Storage.Storages.Users;

public class AddUserStorage(
    WishListDbContext dbContext,
    IMapper mapper)
    : IAddUserStorage
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
}