using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.Exceptions;
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
        if (request.Param.Commands.Length < 2)
            throw new DomainException(BaseMessages.CommandNotRecognized);
            
        if (int.TryParse(request.Param.Commands[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if (wishList is null)
                throw new DomainException(BaseMessages.WishListNotFound);
                
            var sb = new StringBuilder($"Список: *{wishList.Name.MarkForbiddenChar()}*\n");
            sb.AppendLine($"Кол\\-во записей: *{wishList.Presents.Count}*");
            var isPrivate = wishList.IsPrivate ? "вкл" : "выкл";
            sb.AppendLine($"Приватность: *{isPrivate}*");
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "🪪 Название", $"{Commands.WishListEditNameRequest}<?>{wishListId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "🔒 Приватность", $"{Commands.WishListEditSecurityRequest}<?>{wishListId}")
                ],
            ];
            
            keyboard.AddBaseFooter($"{Commands.WishListInfo}<?>{wishListId}");

            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else
        {
            throw new DomainException(BaseMessages.CommandNotRecognized);
        }
    }
}