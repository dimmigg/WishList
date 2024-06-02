using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Domain.UseCases.UpdateUser;

public interface IUpdateUserUseCase
{
    Task<TelegramUser> CreateOrUpdateUser(User user, CancellationToken cancellationToken);
    Task ClearLastCommandUser(long id, CancellationToken cancellationToken);
}