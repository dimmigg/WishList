using Telegram.Bot.Types;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.UpdateUser;

public class UpdateUserUseCase(
    IGetUserStorage getUserStorage,
    IAddUserStorage addUserStorage,
    IUpdateUserStorage updateUserStorage)
    : IUpdateUserUseCase
{
    public async Task CreateOrUpdateUser(User? user, CancellationToken cancellationToken)
    {
        if (user != null)
        {
            var localUser = await getUserStorage.GetUser(user.Id, cancellationToken);
            if (localUser == null)
                await addUserStorage.AddUser(user, cancellationToken);
            else
                await updateUserStorage.UpdateUser(user, cancellationToken);
        }
    }
}