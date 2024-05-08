using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListParams;

public class MyWishListParamsUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage)
    : IRequestHandler<MyWishListParamsCommand>
{
    public async Task Handle(MyWishListParamsCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if (wishList == null) return;
            var sb = new StringBuilder($"Список: *{wishList.Name.MarkForbiddenChar()}*\n");
            sb.AppendLine($"Кол\\-во записей: *{wishList.Presents.Count}*");
            var isPrivate = wishList.IsPrivate ? "вкл" : "выкл";
            sb.AppendLine($"Приватность: *{isPrivate}*");
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "Изменить название", $"{Commands.MY_WISH_LIST_EDIT_NAME_REQUEST}<?>{wishListId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "Изменить приватность", $"{Commands.MY_WISH_LIST_EDIT_SECURITY_REQUEST}<?>{wishListId}")
                ],
            ];
            
            keyboard = keyboard.AddBaseFooter($"{Commands.MY_WISH_LIST_INFO}<?>{wishListId}");

            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}