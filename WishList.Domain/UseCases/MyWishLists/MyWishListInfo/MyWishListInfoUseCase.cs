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
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2)
            throw new DomainException(BaseMessages.CommandNotRecognized);
            
        if (int.TryParse(command[1], out var wishListId))
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
                        "✏️ Список", $"{Commands.Presents}<?>{wishListId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "⚙ Параметры", $"{Commands.WishListParams}<?>{wishListId}")
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