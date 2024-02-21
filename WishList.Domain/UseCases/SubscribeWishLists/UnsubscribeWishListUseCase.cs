using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribeWishLists;

public class UnsubscribeWishListUseCase(
    UseCaseParam param,
    ISender sender,
    IWishListStorage wishListStorage)
    : IUseCase
{

    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.CallbackQuery == null) return;
        var commands = param.Command.Split("</>");
        var lastCommand = commands[^1];
        var command = lastCommand.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if(wishList == null) return;
            
            await wishListStorage.UnsubscribeWishList(param.User.Id, wishListId, cancellationToken);
            var sb = new StringBuilder();
            sb.AppendLine($"Список *{wishList.Name.MarkForbiddenChar()}* удалён из избранного");

            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "« Назад", "subscribe-wish-lists")
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