using WishList.Domain.Exceptions;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Builder;

public class UseCaseBuilder(
    IWishListStorage wishListStorage,
    IUserStorage userStorage,
    IPresentStorage presentStorage,
    ISender sender)
    : IUseCaseBuilder
{
    public IUseCase Build(UseCaseParam param)
    {
        var commands = param.Command.Split("</>");
        var lastCommand = commands[^1].Split("<?>")[0];
        return lastCommand switch
        {
            "main" => new MainUseCase(param, sender),
            "my-wish-lists" => new MyWishListsUseCase(param, sender, wishListStorage),
            "my-wish-list-name-request" => new MyWishListNameRequestUseCase(param, sender, userStorage),
            "my-wish-list-add" => new MyWishListAddUseCase(param, sender, wishListStorage, userStorage),
            "my-wish-list-info" => new MyWishListInfoUseCase(param, sender, wishListStorage),
            "my-presents" => new MyPresentsUseCase(param, sender, wishListStorage, presentStorage),
            _ => throw new DomainException("Команда не распознана")
        };
    }
}