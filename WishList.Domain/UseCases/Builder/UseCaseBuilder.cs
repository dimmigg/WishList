using WishList.Domain.Exceptions;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Builder;

public class UseCaseBuilder(
    IUserStorage userStorage,
    IWishListStorage wishListStorage,
    ISender sender)
    : IUseCaseBuilder
{
    public IUseCase Build(BuildParam param)
    {
        param.Command.ParseCommand(out var commands);
        var lastCommand = commands[^1];
        var command = lastCommand.Split("<?>");
        return lastCommand switch
        {
            "main" => new StartUseCase(param, sender),
            "my-wish-lists" => new GetWishListsUseCase(param, sender, wishListStorage),
            _ => throw new DomainException("Команда не распознана")
        };
    }
}