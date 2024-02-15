using WishList.Domain.Exceptions;
using WishList.Domain.Received.CallbackQueryReceived.CreateWishList;
using WishList.Domain.TelegramSender;
using WishList.Storage.CommandOptions;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.Received.CallbackQueryReceived;

public class CallbackQueryBuilder(
    IUserStorage userStorage,
    ISender sender)
    : ICallbackQueryBuilder
{
    public ICallbackReceived Build(string command, CancellationToken cancellationToken)
    {
        var commands = command.Split('/');
        if (!Enum.TryParse<Command>(commands[0], out var way) ||
            !Enum.TryParse<CommandStep>(commands[1], out var step))
            throw new DomainException("Команда не распознана");
        switch (way)
        {
            case Command.CreateWishList:
                return new CreateWishListCallbackReceived(way, step, userStorage, sender);
            case Command.Null:
            default:
                throw new DomainException("Команда не распознана");
        }
    }
}