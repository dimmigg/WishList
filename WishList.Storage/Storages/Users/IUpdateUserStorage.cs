using Telegram.Bot.Types;

namespace WishList.Storage.Storages.Users;

public interface IUpdateUserStorage
{
    Task UpdateUser(User user, CancellationToken cancellationToken);
}