using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Presents;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyPresents.MyPresentInfo;

public class MyPresentInfoUseCase(
    ISender sender,
    IPresentStorage presentStorage)
    : IRequestHandler<MyPresentInfoCommand>
{
    public async Task Handle(MyPresentInfoCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

        var command = request.Param.Command.Split("<?>");
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
                        "Изменить название", $"{Commands.MY_PRESENT_EDIT_NAME_REQUEST}<?>{presentId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "Изменить ссылку", $"{Commands.MY_PRESENT_EDIT_REFERENCE_REQUEST}<?>{presentId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "Изменить комментарий", $"{Commands.MY_PRESENT_EDIT_COMMENT_REQUEST}<?>{presentId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "Удалить", $"{Commands.MY_PRESENT_DELETE_REQUEST}<?>{presentId}")
                ],
            ];
            
            keyboard = keyboard.AddBaseFooter($"{Commands.MY_PRESENTS}<?>{present.WishListId}");

            var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
            var messageId = request.Param.CallbackQuery.Message?.MessageId;
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