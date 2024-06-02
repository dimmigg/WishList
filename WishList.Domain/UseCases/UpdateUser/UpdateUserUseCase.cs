using Telegram.Bot.Types;
using WishList.Storage.Entities;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.UpdateUser;

public class UpdateUserUseCase(
    IUserStorage userStorage)
    : IUpdateUserUseCase
{
    public async Task<TelegramUser> CreateOrUpdateUser(User user, CancellationToken cancellationToken)
    {
        var tgUser = await userStorage.UpdateUser(user, cancellationToken);
        return tgUser;
    }

    public Task ClearLastCommandUser(long id, CancellationToken cancellationToken) =>
        userStorage.UpdateLastCommandUser(id, null, cancellationToken);
}