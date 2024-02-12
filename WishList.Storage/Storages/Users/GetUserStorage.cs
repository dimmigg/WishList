using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace WishList.Storage.Storages.Users;

public class GetUserStorage(
    WishListDbContext dbContext,
    IMapper mapper) : IGetUserStorage
{
    public async Task<User?> GetUser(long id, CancellationToken cancellationToken)
    {
        var query = dbContext.Users.Where(u => u.Id == id);
        var user = await query.FirstOrDefaultAsync(cancellationToken);
        return mapper.Map<User>(user);
    }
}