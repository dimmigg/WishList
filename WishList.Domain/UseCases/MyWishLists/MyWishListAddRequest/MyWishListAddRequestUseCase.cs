using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListAddRequest;

public class MyWishListAddRequestUseCase(
    ITelegramSender telegramSender,
    IUpdateUserUseCase updateUserUseCase
    ) : IRequestHandler<MyWishListAddRequestCommand>
{
    public async Task Handle(MyWishListAddRequestCommand request, CancellationToken cancellationToken)
    {
        const string textMessage = "Введите название нового списка";

        updateUserUseCase.UpdateLastCommandUser(request.Param.User.Id, Commands.WishListAdd);
        
        var keyboard = new List<List<InlineKeyboardButton>>();
        keyboard.AddSelfDeleteButton();

        await telegramSender.AnswerCallbackQueryAsync(
            text: textMessage,
            cancellationToken: cancellationToken);

        await telegramSender.SendMessageAsync(
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}