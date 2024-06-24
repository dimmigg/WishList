using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListInfo;

public class MyWishListInfoUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage)
    : IRequestHandler<MyWishListInfoCommand>
{
    public async Task Handle(MyWishListInfoCommand request, CancellationToken cancellationToken)
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
                        "📝 Изменить список", $"{Commands.Presents}<?>{wishListId}"),
                    
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "✏️ Переименовать", $"{Commands.WishListEditNameRequest}<?>{wishListId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "🗑 Удалить", $"{Commands.WishListDeleteRequest}<?>{wishListId}")
                ],
            ];

            keyboard.AddBaseFooter(Commands.WishLists);

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