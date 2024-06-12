using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types;
using WishList.Storage.Entities;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.UpdateUser;

public class UpdateUserUseCase(
    IUserStorage userStorage,
    IMemoryCache cache)
    : IUpdateUserUseCase
{
    private readonly TimeSpan cacheDuration = TimeSpan.FromHours(24);
    public async Task<TelegramUser> CreateOrUpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        if (cache.TryGetValue(user.Id, out TelegramUser tgUser)) return tgUser;
        tgUser = await userStorage.UpdateUser(user, cancellationToken);
        cache.Set(user.Id, tgUser, cacheDuration);
        
        return tgUser;
    }

    public void UpdateLastCommandUser(long id, string? command = null)
    {
        var user = cache.Get<TelegramUser>(id)!;
        user.LastCommand = command;
        cache.Set(user.Id, user, cacheDuration);
    }
}