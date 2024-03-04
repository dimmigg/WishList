using Telegram.Bot.Types;
using WishList.Domain.Models;

namespace WishList.Domain.UseCases.UpdateUser;

public interface IUpdateUserUseCase
{
    Task<RegisteredUser> CreateOrUpdateUser(User user, CancellationToken cancellationToken);
    Task ClearLastCommandUser(long id, CancellationToken cancellationToken);
}