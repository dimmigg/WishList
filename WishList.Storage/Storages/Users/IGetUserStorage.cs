using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Storage.Storages.Users;

public interface IGetUserStorage
{
    Task<User?> GetUser(long id, CancellationToken cancellationToken);
}