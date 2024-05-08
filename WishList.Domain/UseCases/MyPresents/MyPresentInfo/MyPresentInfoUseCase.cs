﻿using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
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
                        "🪪 Название", $"{Commands.MY_PRESENT_EDIT_NAME_REQUEST}<?>{presentId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "📝 Ссылка", $"{Commands.MY_PRESENT_EDIT_REFERENCE_REQUEST}<?>{presentId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "📄 Комментарий", $"{Commands.MY_PRESENT_EDIT_COMMENT_REQUEST}<?>{presentId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "🗑 Удалить", $"{Commands.MY_PRESENT_DELETE_REQUEST}<?>{presentId}")
                ],
            ];
            
            keyboard = keyboard.AddBaseFooter($"{Commands.MY_PRESENTS}<?>{present.WishListId}");

            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}