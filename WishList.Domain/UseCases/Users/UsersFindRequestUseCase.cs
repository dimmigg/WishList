using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.Users;

public class UsersFindRequestUseCase(
    UseCaseParam param,
    ISender sender,
    IUserStorage userStorage)
    : IUseCase
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.CallbackQuery == null) return;

        const string textMessage = "Введи идентификатор или логин пользователя, чей список нужно добавить";

        await userStorage.UpdateLastCommandUser(param.User.Id, "uf", cancellationToken);

        List<List<InlineKeyboardButton>> keyboard =
        [
            [
                InlineKeyboardButton.WithCallbackData(
                    "« Главное меню", "main")
            ]
        ];

        var chatId = param.CallbackQuery.Message?.Chat.Id;
        var messageId = param.CallbackQuery.Message?.MessageId;
        if (!(chatId.HasValue && messageId.HasValue)) return;
        await sender.EditMessageTextAsync(
            chatId: chatId.Value,
            messageId: messageId.Value,
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}