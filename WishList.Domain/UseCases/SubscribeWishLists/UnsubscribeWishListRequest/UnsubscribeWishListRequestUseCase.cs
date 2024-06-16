using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribeWishLists.UnsubscribeWishListRequest;

public class UnsubscribeWishListRequestUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage)
    : IRequestHandler<UnsubscribeWishListRequestCommand>
{
    public async Task Handle(UnsubscribeWishListRequestCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if(wishList == null) return;
            
            var sb = new StringBuilder();
            sb.AppendLine($"Вы действительно хотите удалить список *{wishList.Name.MarkForbiddenChar()}* из избранного?\n" +
                          $"Все резервы будут удалены\\.");

            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "👌 Да", $"{Commands.UnsubscribeWishList}<?>{wishListId}")
                ],
            ];

            keyboard.AddBaseFooter($"{Commands.SubscribeWishListInfo}<?>{wishListId}");

            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}