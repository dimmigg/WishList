using WishList.Domain.Exceptions;
using WishList.Domain.Received.CallbackQueryReceived.CreateWishList;
using WishList.Domain.TelegramSender;
using WishList.Storage.CommandOptions;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.Received.CallbackQueryReceived;

public class CallbackQueryBuilder(
    IUserStorage userStorage,
    IWishListStorage wishListStorage,
    ISender sender)
    : ICallbackQueryBuilder
{
    public ICallbackReceived Build(Command way, CommandStep step, CancellationToken cancellationToken)
    {
        switch (way)
        {
            case Command.CreateWishList:
                return new CreateWishListCallbackReceived(way, step, userStorage, wishListStorage, sender);
            case Command.Null:
            default:
                throw new DomainException("Команда не распознана");
        }
    }
}