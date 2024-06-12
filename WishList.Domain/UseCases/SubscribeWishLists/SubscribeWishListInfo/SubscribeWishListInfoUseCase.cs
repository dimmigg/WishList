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
            if (wishList is null) return;
            var sb = new StringBuilder($"Список: *{wishList.Name.MarkForbiddenChar()}*\n");
            sb.AppendLine($"Кол\\-во записей: *{wishList.Presents.Count}*");
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "🧾 Список желаний", $"{Commands.SubscribePresents}<?>{wishListId}")
                ],
            ];

            var presents = await presentStorage.GetPresents(wishList.Id, cancellationToken);

            if (presents.Any(p => p?.ReserveForUserId == request.Param.User.Id))
            {
                keyboard.Add([
                    InlineKeyboardButton.WithCallbackData(
                        "📌 Мои резервы", $"{Commands.SubscribePresents}<?>{wishListId}<?>{Commands.Reserved}")
                ]);
            }
            keyboard.Add([
                InlineKeyboardButton.WithCallbackData(
                    "👋 Отписаться", $"{Commands.UnsubscribeWishListRequest}<?>{wishListId}")
            ]);

            keyboard = keyboard.AddBaseFooter($"{Commands.SubscribeUserWishLists}<?>{wishList.AuthorId}");
            
            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}