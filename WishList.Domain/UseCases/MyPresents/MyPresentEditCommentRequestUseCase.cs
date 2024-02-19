using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.MyPresents;

public class MyPresentEditCommentRequestUseCase(
    UseCaseParam param,
    ISender sender,
    IUserStorage userStorage)
    : IUseCase
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.CallbackQuery == null) return;
        var commands = param.Command.Split("</>");
        var lastCommand = commands[^1];
        var command = lastCommand.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {

            const string textMessage = "Введите комментарий записи";

            await userStorage.UpdateLastCommandUser(param.User.Id, $"my-present-edit-comment<?>{presentId}", cancellationToken);

            var chatId = param.CallbackQuery.Message?.Chat.Id;
            if (!chatId.HasValue) return;
            await sender.SendTextMessageAsync(
                chatId: chatId.Value,
                text: textMessage,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }

}