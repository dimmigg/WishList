using Telegram.Bot.Types;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.UpdateUser;

public class UpdateUserUseCase(
    IUserStorage userStorage)
    : IUpdateUserUseCase
{
    public async Task CreateOrUpdateUser(User? user, CancellationToken cancellationToken)
    {
        if (user != null)
        {
            var localUser = await userStorage.GetUser(user.Id, cancellationToken);
            if (localUser == null)
                await userStorage.AddUser(user, cancellationToken);
            else
                await userStorage.UpdateUser(user, cancellationToken);
        }
    }
}