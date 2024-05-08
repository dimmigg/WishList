using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.MyPresents.MyPresentEditNameRequest;

public class MyPresentEditNameRequestUseCase(
    ITelegramSender telegramSender,
    IUserStorage userStorage)
    : IRequestHandler<MyPresentEditNameRequestCommand>
{
    public async Task Handle(MyPresentEditNameRequestCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            const string textMessage = "Введите название записи";

            await userStorage.UpdateLastCommandUser(request.Param.User.Id, $"{Commands.MY_PRESENT_EDIT_NAME}<?>{presentId}", cancellationToken);
            
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