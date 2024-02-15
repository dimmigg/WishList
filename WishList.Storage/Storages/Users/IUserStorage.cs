using Telegram.Bot.Types;
using WishList.Storage.CommandOptions;
using WishList.Storage.Entities;

namespace WishList.Storage.Storages.Users;

public interface IUserStorage
{
    Task<TelegramUser> AddUser(User user, CancellationToken cancellationToken);
    Task<TelegramUser?> GetUser(long id, CancellationToken cancellationToken);
    Task<TelegramUser> UpdateUser(User user, CancellationToken cancellationToken);
    Task<TelegramUser> UpdateWayUser(long id, Command command, CommandStep commandStep, CancellationToken cancellationToken);
}