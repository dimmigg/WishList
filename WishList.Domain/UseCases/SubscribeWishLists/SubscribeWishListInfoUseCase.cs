using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribeWishLists;

public class SubscribeWishListInfoUseCase(
    UseCaseParam param,
    ISender sender,
    IWishListStorage wishListStorage,
    IPresentStorage presentStorage)
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
            if (wishList == null) return;
            var sb = new StringBuilder($"Список: *{wishList.Name.MarkForbiddenChar()}*\n");
            sb.AppendLine($"Кол\\-во записей: *{wishList.Presents.Count}*");
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "Список желаний", $"sp<?>{wishListId}")
                ],
            ];

            var presents = await presentStorage.GetPresents(wishList.Id, cancellationToken);

            if (presents.Any(p => p?.ReserveForUserId == param.User.Id))
            {
                keyboard.Add([
                    InlineKeyboardButton.WithCallbackData(
                        "Мои резервы", $"sp<?>{wishListId}<?>r")
                ]);
            }
            keyboard.Add([
                InlineKeyboardButton.WithCallbackData(
                    "Отписаться", $"uwlr<?>{wishListId}")
            ]);
            keyboard.Add([
                InlineKeyboardButton.WithCallbackData(
                    "« Назад", $"swl"),
                InlineKeyboardButton.WithCallbackData(
                    "« Главное меню", "main")
            ]);
            
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