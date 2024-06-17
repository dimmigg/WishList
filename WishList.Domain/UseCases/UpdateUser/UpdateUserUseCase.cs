using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types;
using WishList.Domain.Exceptions;
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
        if (!cache.TryGetValue(user.Id, out TelegramUser? tgUser))
            return await UpdateUserAsync(user, cancellationToken);
        if (tgUser is not null)
            return tgUser;

        return await UpdateUserAsync(user, cancellationToken);
    }

    public async Task<TelegramUser> UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        var tgUser = await userStorage.UpdateUser(user, cancellationToken);
        cache.Set(user.Id, tgUser, cacheDuration);

        return tgUser;
    }

    public async Task RefreshUser(long userId, CancellationToken cancellationToken)
    {
        var user = await userStorage.GetMainUser(userId, cancellationToken);
        if (user is null)
            throw new DomainException(Constants.BaseMessages.UserNotFound);
        cache.Set(user.Id, user, cacheDuration);
    }

    public void UpdateLastCommandUser(long id, string? command = null)
    {
        var user = cache.Get<TelegramUser>(id)!;
        user.LastCommand = command;
        cache.Set(user.Id, user, cacheDuration);
    }
}