using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListAdd;

public class MyWishListAddUseCase(
    ISender sender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage
    )
    : IRequestHandler<MyWishListAddCommand>
{
    public async Task Handle(MyWishListAddCommand request, CancellationToken cancellationToken)
    {
        if(request.Param.Message == null || string.IsNullOrWhiteSpace(request.Param.Message.Text)) return;
        
        const string textMessage = "Отлично\\! Список добавлен";

        await userStorage.UpdateLastCommandUser(request.Param.User.Id, null, cancellationToken);
        await wishListStorage.AddWishList(request.Param.Message.Text, request.Param.User.Id, cancellationToken);

        var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter(Commands.MY_WISH_LISTS);
        
        await sender.SendTextMessageAsync(
            chatId: request.Param.Message.Chat.Id,
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}