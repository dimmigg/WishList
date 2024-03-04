using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.MyPresents;

public class MyPresentInfoUseCase(
    UseCaseParam param,
    ISender sender,
    IPresentStorage presentStorage)
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
            var present = await presentStorage.GetPresent(presentId, cancellationToken);
            if (present == null) return;
            var sb = new StringBuilder($"Запись: *{present.Name.MarkForbiddenChar()}*\n");
            var reference = string.IsNullOrWhiteSpace(present.Reference) ?
                "_не заполнено_"
                : $"[тык]({present.Reference.MarkForbiddenChar()})";
            sb.AppendLine($"Ссылка: *{reference}*");
            var comment = string.IsNullOrWhiteSpace(present.Comment) ?
                "_не заполнено_"
                : present.Comment.MarkForbiddenChar();
            sb.AppendLine($"Комментарий: *{comment}*");
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "Изменить название", $"my-present-edit-name-request<?>{presentId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "Изменить ссылку", $"my-present-edit-reference-request<?>{presentId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "Изменить комментарий", $"my-present-edit-comment-request<?>{presentId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "Удалить", $"my-present-delete-request<?>{presentId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "« Назад", $"my-presents<?>{present.WishListId}"),
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
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}