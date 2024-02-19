using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Storage.Storages.Users;

public interface IUserStorage
{
    Task<TelegramUser> AddUser(User user, CancellationToken cancellationToken);
    Task<TelegramUser?> GetUser(long id, CancellationToken cancellationToken);
    Task<TelegramUser> UpdateUser(User user, CancellationToken cancellationToken);
    Task<TelegramUser> UpdateLastCommandUser(long id, string? command, CancellationToken cancellationToken);
    Task<TelegramUser[]?> FindUsers(string findText, CancellationToken cancellationToken);
}