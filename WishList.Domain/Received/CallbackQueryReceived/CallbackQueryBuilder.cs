using WishList.Domain.Exceptions;
using WishList.Domain.Received.CallbackQueryReceived.CreateWishList;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.WayOptions;

namespace WishList.Domain.Received.CallbackQueryReceived;

public class CallbackQueryBuilder(
    IUserStorage userStorage,
    ISender sender)
    : ICallbackQueryBuilder
{
    public ICallbackReceived Build(string command, CancellationToken cancellationToken)
    {
        var commands = command.Split('/');
        if (!Enum.TryParse<Way>(commands[0], out var way) ||
            !Enum.TryParse<StepWay>(commands[1], out var step))
            throw new DomainException("Команда не распознана");
        switch (way)
        {
            case Way.CreateWishList:
                return new CreateWishListCallbackReceived(way, step, userStorage, sender);
            case Way.Null:
            default:
                throw new DomainException("Команда не распознана");
        }
    }
}