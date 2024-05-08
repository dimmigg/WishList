using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListAdd;

public class MyWishListAddUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage
    )
    : IRequestHandler<MyWishListAddCommand>
{
    public async Task Handle(MyWishListAddCommand request, CancellationToken cancellationToken)
    {
        const string textMessage = "Отлично\\! Список добавлен";

        await userStorage.UpdateLastCommandUser(request.Param.User.Id, null, cancellationToken);
        await wishListStorage.AddWishList(request.Param.Message!.Text!, request.Param.User.Id, cancellationToken);

        var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter(Commands.MY_WISH_LISTS);
        
        await telegramSender.SendMessageAsync(
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}