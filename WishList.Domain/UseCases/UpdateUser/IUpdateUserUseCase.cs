using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Domain.UseCases.UpdateUser;

public interface IUpdateUserUseCase
{
    Task<TelegramUser> CreateOrUpdateUserAsync(User user, CancellationToken cancellationToken);
    void UpdateLastCommandUser(long id, string? command = null);
    Task<TelegramUser> UpdateUserAsync(User user, CancellationToken cancellationToken);
    Task RefreshUser(long userId, CancellationToken cancellationToken);
}