using Telegram.Bot.Types;

namespace WishList.Storage.Storages.Users;

public interface IAddUserStorage
{
    Task<User> AddUser(User user, CancellationToken cancellationToken);
}