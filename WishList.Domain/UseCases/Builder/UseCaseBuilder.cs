using WishList.Domain.Exceptions;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Builder;

public class UseCaseBuilder(
    IWishListStorage wishListStorage,
    ISender sender)
    : IUseCaseBuilder
{
    public IUseCase Build(UseCaseParam param)
    {
        var commands = param.Command.Split("</>");
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