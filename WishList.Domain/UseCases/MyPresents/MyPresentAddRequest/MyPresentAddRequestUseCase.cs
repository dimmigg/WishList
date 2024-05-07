using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Users;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyPresents.MyPresentAddRequest;

public class MyPresentAddRequestUseCase(
    ISender sender,
    IUserStorage userStorage)
    : IRequestHandler<MyPresentAddRequestCommand>
{
    public async Task Handle(MyPresentAddRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            const string textMessage = "Введите название новой записи";

            await userStorage.UpdateLastCommandUser(request.Param.User.Id, $"{Commands.MY_PRESENT_ADD}<?>{wishListId}", cancellationToken);
            
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