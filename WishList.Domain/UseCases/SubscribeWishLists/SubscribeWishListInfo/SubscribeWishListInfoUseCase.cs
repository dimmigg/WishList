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
        if (request.Param.Commands.Length < 2) return;
        if (int.TryParse(request.Param.Commands[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if (wishList is null) return;
            var sb = new StringBuilder(
                $"Список: *{wishList.Name.MarkForbiddenChar()}* _\\({wishList.Author.ToString().MarkForbiddenChar()}\\)_\n");
            sb.AppendLine($"Кол\\-во записей: *{wishList.Presents.Count}*");

            if (wishList.Presents.Count != 0)
            {
                sb.AppendLine();
                foreach (var present in wishList.Presents)
                {
                    sb.AppendLine($"\\-\t *{present.Name.MarkForbiddenChar()}*");
                }
            }

            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "👀 Смотреть список", $"{Commands.SubscribePresents}<?>{wishListId}")
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

            var wishLists = (await wishListStorage.GetSubscribeWishLists(request.Param.User.Id, cancellationToken))
                .Where(wl => wl.AuthorId == wishList.AuthorId)
                .ToArray();

            keyboard.AddBaseFooter(wishLists.Length == 1
                ? Commands.SubscribeUsers
                : $"{Commands.SubscribeUserWishLists}<?>{wishList.AuthorId}");

            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}