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

        var command = param.Command.Split("<?>");
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
                        "Изменить название", $"mpenr<?>{presentId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "Изменить ссылку", $"mperr<?>{presentId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "Изменить комментарий", $"mpecr<?>{presentId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "Удалить", $"mpdr<?>{presentId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "« Назад", $"mp<?>{present.WishListId}"),
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