using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Users;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyPresents.MyPresentEditCommentRequest;

public class MyPresentEditCommentRequestUseCase(
    ISender sender,
    IUserStorage userStorage)
    : IRequestHandler<MyPresentEditCommentRequestCommand>
{
    public async Task Handle(MyPresentEditCommentRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {

            const string textMessage = "Введите комментарий записи";

            await userStorage.UpdateLastCommandUser(request.Param.User.Id, $"{Commands.MY_PRESENT_EDIT_COMMENT}<?>{presentId}", cancellationToken);

            var keyboard = new List<List<InlineKeyboardButton>>().AddSelfDeleteButton();

            var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
            if (!chatId.HasValue) return;
            
            await sender.AnswerCallbackQueryAsync(
                callbackQueryId: request.Param.CallbackQuery.Id,
                text: textMessage,
                cancellationToken: cancellationToken);
            
            await sender.SendTextMessageAsync(
                chatId: chatId.Value,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}