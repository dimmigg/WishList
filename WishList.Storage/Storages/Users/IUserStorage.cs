using Telegram.Bot.Types;

namespace WishList.Storage.Storages.Users;

public interface IUserStorage
{
    Task<User> AddUser(User user, CancellationToken cancellationToken);
    Task<User?> GetUser(long id, CancellationToken cancellationToken);
    Task UpdateUser(User user, CancellationToken cancellationToken);
}