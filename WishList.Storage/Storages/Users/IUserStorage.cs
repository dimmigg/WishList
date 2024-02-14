using Telegram.Bot.Types;
using WishList.Storage.Entities;
using WishList.Storage.WayOptions;

namespace WishList.Storage.Storages.Users;

public interface IUserStorage
{
    Task<TelegramUser> AddUser(User user, CancellationToken cancellationToken);
    Task<TelegramUser?> GetUser(long id, CancellationToken cancellationToken);
    Task<TelegramUser> UpdateUser(User user, CancellationToken cancellationToken);
    Task<TelegramUser> UpdateWayUser(long id, Way way, StepWay step, CancellationToken cancellationToken);
}