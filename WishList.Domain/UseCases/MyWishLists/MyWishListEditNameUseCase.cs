using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists;

public class MyWishListEditNameUseCase(
    UseCaseParam param,
    ISender sender,
    IWishListStorage wishListStorage,
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
            const string textMessage = @"Отлично\! Название списока обновлено\!";

            await userStorage.UpdateLastCommandUser(param.User.Id, null, cancellationToken);
            await wishListStorage.EditNameWishList(param.Message.Text, wishListId, cancellationToken);

            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "« Назад", $"my-wish-lists"),
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