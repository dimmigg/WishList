using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribeWishLists.SubscribeWishListInfo;

public class SubscribeWishListInfoUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IPresentStorage presentStorage)
    : IRequestHandler<SubscribeWishListInfoCommand>
{
    public async Task Handle(SubscribeWishListInfoCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
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
                        "🧾 Список желаний", $"{Commands.SUBSCRIBE_PRESENTS}<?>{wishListId}")
                ],
            ];

            var presents = await presentStorage.GetPresents(wishList.Id, cancellationToken);

            if (presents.Any(p => p?.ReserveForUserId == request.Param.User.Id))
            {
                keyboard.Add([
                    InlineKeyboardButton.WithCallbackData(
                        "📌 Мои резервы", $"{Commands.SUBSCRIBE_PRESENTS}<?>{wishListId}<?>{Commands.RESERVED}")
                ]);
            }
            keyboard.Add([
                InlineKeyboardButton.WithCallbackData(
                    "👋 Отписаться", $"{Commands.UNSUBSCRIBE_WISH_LIST_REQUEST}<?>{wishListId}")
            ]);

            keyboard = keyboard.AddBaseFooter(Commands.SUBSCRIBE_WISH_LISTS);
            
            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}