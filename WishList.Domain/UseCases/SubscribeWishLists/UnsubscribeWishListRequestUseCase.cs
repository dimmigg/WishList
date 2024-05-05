using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribeWishLists;

public class UnsubscribeWishListRequestUseCase(
    UseCaseParam param,
    ISender sender,
    IWishListStorage wishListStorage)
    : IUseCase
{

    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.CallbackQuery == null) return;

        var command = param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if(wishList == null) return;
            
            await wishListStorage.UnsubscribeWishList(param.User.Id, wishListId, cancellationToken);
            var sb = new StringBuilder();
            sb.AppendLine($"Вы действительно хотите удалить список *{wishList.Name.MarkForbiddenChar()}* из избранного?");

            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "Да", $"uwl<?>{wishListId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "« Назад", $"swli<?>{wishListId}")
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