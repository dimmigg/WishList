using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.WishLists;
using Present = WishList.Storage.Entities.Present;

namespace WishList.Domain.UseCases.SubscribePresents;

public class SubscribePresentsUseCase(
    UseCaseParam param,
    ISender sender,
    IPresentStorage presentStorage,
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

            List<List<InlineKeyboardButton>> keyboard = [];
            var wishLists = await presentStorage.GetSubscribePresents(wishListId, cancellationToken);
            
            var sb = new StringBuilder();
            if (wishLists.Length != 0)
            {
                if (command.Length == 3)
                {
                    sb.AppendLine($"Мои резервы из списка *{wishList.Name.MarkForbiddenChar()}*:");
                    wishLists = wishLists.Where(p => p.ReserveForUserId == param.User.Id).ToArray();
                    keyboard = wishLists
                        .Select(present =>
                        {
                            var isReserve = present.ReserveForUserId.HasValue
                                ? present.ReserveForUserId.Value == param.User.Id ? "🟡 " : "🔴 "
                                : "🟢 ";
                            return new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData($"{isReserve}{present.Name}",
                                $"spi<?>{present.Id}<?>r"),
                            };
                        }).ToList();
                }
                else
                {
                    sb.AppendLine($"Список *{wishList.Name.MarkForbiddenChar()}*:");
                    keyboard = wishLists
                        .Select(present =>
                        {
                            var isReserve = present.ReserveForUserId.HasValue
                                ? present.ReserveForUserId.Value == param.User.Id ? "🟡 " : "🔴 "
                                : "🟢 ";
                            return new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData($"{isReserve}{present.Name}",
                                    $"spi<?>{present.Id}"),
                            };
                        }).ToList();
                }
            }
            else
            {
                sb.AppendLine($"Список *{wishList.Name.MarkForbiddenChar()}* пуст");
            }

            keyboard.Add([
                InlineKeyboardButton.WithCallbackData(
                    "« Назад", $"swli<?>{wishList.Id}")
            ]);
            keyboard.Add([
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