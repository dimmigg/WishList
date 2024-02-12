using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Storage.Storages.Users;

public class UpdateUserStorage(
    WishListDbContext dbContext,
    IMapper mapper)
    : IUpdateUserStorage
{
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