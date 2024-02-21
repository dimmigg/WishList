using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribePresents;

public class SubscribePresentInfoUseCase(
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
            if(present == null) return; 
            
            var sb = new StringBuilder();
            sb.AppendLine($"Подарок: *{present.Name.MarkForbiddenChar()}*");
            sb.AppendLine($"Ссылка: *{present.Reference?.MarkForbiddenChar()}*");
            sb.AppendLine($"Комментарий: *{present.Comment?.MarkForbiddenChar()}*");
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "« Назад", $"subscribe-presents<?>{present.WishListId}")
                ],
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
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}