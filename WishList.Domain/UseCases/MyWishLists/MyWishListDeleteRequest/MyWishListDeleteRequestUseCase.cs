using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListDeleteRequest;

public class MyWishListDeleteRequestUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage)
    : IRequestHandler<MyWishListDeleteRequestCommand>
{
    public async Task Handle(MyWishListDeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2)
            throw new DomainException(BaseMessages.COMMAND_NOT_RECOGNIZED);
        
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            
            if (wishList == null)
                throw new DomainException(BaseMessages.WISH_LIST_NOT_FOUND);
            
            var textMessage = $"Удалить список *{wishList.Name.MarkForbiddenChar()}*?";
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [InlineKeyboardButton.WithCallbackData(
                        "👌 Да", $"{Commands.MY_WISH_LIST_DELETE}<?>{wishList.Id}"),
                ],
            ];
            keyboard = keyboard.AddBaseFooter($"{Commands.MY_WISH_LIST_INFO}<?>{wishList.Id}");
            
            await telegramSender.EditMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else
        {
            throw new DomainException(BaseMessages.COMMAND_NOT_RECOGNIZED);
        }
    }
}