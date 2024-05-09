using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListAddRequest;

public class MyWishListAddRequestUseCase(
    ITelegramSender telegramSender,
    IUserStorage userStorage)
    : IRequestHandler<MyWishListAddRequestCommand>
{
    public async Task Handle(MyWishListAddRequestCommand request, CancellationToken cancellationToken)
    {
        const string textMessage = "Введите название нового списка";

        await userStorage.UpdateLastCommandUser(request.Param.User.Id, Commands.MY_WISH_LIST_ADD, cancellationToken);
        
        var keyboard = new List<List<InlineKeyboardButton>>().AddSelfDeleteButton();

        await telegramSender.AnswerCallbackQueryAsync(
            text: textMessage,
            cancellationToken: cancellationToken);

        await telegramSender.SendMessageAsync(
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}