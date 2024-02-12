using Telegram.Bot.Types;

namespace WishList.Domain.UseCases.UpdateUser;

public interface IUpdateUserUseCase
{
    Task CreateOrUpdateUser(User? user, CancellationToken cancellationToken);
}