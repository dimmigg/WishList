using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.MyPresents.MyPresentEditCommentRequest;

public class MyPresentEditCommentRequestUseCase(
    ITelegramSender telegramSender,
    IUserStorage userStorage)
    : IRequestHandler<MyPresentEditCommentRequestCommand>
{
    public async Task Handle(MyPresentEditCommentRequestCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {

            const string textMessage = "Введите комментарий записи";

            await userStorage.UpdateLastCommandUser(request.Param.User.Id, $"{Commands.MY_PRESENT_EDIT_COMMENT}<?>{presentId}", cancellationToken);

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
}