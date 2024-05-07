using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Users;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListAddRequest;

public class MyWishListAddRequestUseCase(
    ISender sender,
    IUserStorage userStorage)
    : IRequestHandler<MyWishListAddRequestCommand>
{
    public async Task Handle(MyWishListAddRequestCommand request, CancellationToken cancellationToken)
    {
        if(request.Param.CallbackQuery == null) return;
        
        const string textMessage = "Введите название нового списка";

        await userStorage.UpdateLastCommandUser(request.Param.User.Id, Commands.MY_WISH_LIST_ADD, cancellationToken);
        
        var keyboard = new List<List<InlineKeyboardButton>>().AddSelfDeleteButton();

        await sender.AnswerCallbackQueryAsync(
            callbackQueryId: request.Param.CallbackQuery.Id,
            text: textMessage,
            cancellationToken: cancellationToken);

        var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
        if(!chatId.HasValue) return;
        await sender.SendTextMessageAsync(
            chatId: chatId.Value,
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}