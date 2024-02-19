using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.MyPresents;

public class MyPresentAddUseCase(
    UseCaseParam param,
    ISender sender,
    IPresentStorage presentStorage,
    IUserStorage userStorage
    )
    : IUseCase
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.Message == null || string.IsNullOrWhiteSpace(param.Message.Text)) return;

        var commands = param.Command.Split("</>");
        var lastCommand = commands[^1];
        var command = lastCommand.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            const string textMessage = "Отлично\\! Запись добавлена";

            await userStorage.UpdateLastCommandUser(param.User.Id, null, cancellationToken);
            await presentStorage.AddPresent(param.Message.Text, wishListId, cancellationToken);

            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "« Назад", $"my-presents<?>{wishListId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "« Главное меню", "main")
                ]
            ];

            await sender.SendTextMessageAsync(
                chatId: param.Message.Chat.Id,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }

}