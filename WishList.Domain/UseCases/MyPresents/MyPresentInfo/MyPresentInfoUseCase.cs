using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.MyPresents.MyPresentInfo;

public class MyPresentInfoUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage)
    : IRequestHandler<MyPresentInfoCommand>
{
    public async Task Handle(MyPresentInfoCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            var present = await presentStorage.GetPresent(presentId, cancellationToken);
            if (present is null) return;
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
                        "🪪 Название", $"{Commands.PresentEditNameRequest}<?>{presentId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "📝 Ссылка", $"{Commands.PresentEditReferenceRequest}<?>{presentId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "📄 Комментарий", $"{Commands.PresentEditCommentRequest}<?>{presentId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "🗑 Удалить", $"{Commands.PresentDeleteRequest}<?>{presentId}")
                ],
            ];
            
            keyboard = keyboard.AddBaseFooter($"{Commands.Presents}<?>{present.WishListId}");

            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}