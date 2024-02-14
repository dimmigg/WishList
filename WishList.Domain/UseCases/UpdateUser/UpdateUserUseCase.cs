using AutoMapper;
using Telegram.Bot.Types;
using WishList.Domain.Models;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.UpdateUser;

public class UpdateUserUseCase(
    IUserStorage userStorage,
    IMapper mapper)
    : IUpdateUserUseCase
{
    public async Task<RegisteredUser> CreateOrUpdateUser(User user, CancellationToken cancellationToken)
    {
        var result = await userStorage.UpdateUser(user, cancellationToken);
        return mapper.Map<RegisteredUser>(result);
    }
}