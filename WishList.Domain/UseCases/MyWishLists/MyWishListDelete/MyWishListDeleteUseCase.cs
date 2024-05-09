using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListDelete;

public class MyWishListDeleteUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage)
    : IRequestHandler<MyWishListDeleteCommand>
{
    public async Task Handle(MyWishListDeleteCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");

        if (command.Length < 2)
        {
            await telegramSender.ShowAlertAsync(BaseMessages.COMMAND_NOT_RECOGNIZED, cancellationToken);
            return;
        }
        
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);

            if (wishList == null)
            {
                await telegramSender.ShowAlertAsync(BaseMessages.WISH_LIST_NOT_FOUND, cancellationToken);
                return;
            }
            
            await wishListStorage.Delete(wishListId, cancellationToken);

            var textMessage = $"Список *{wishList.Name.MarkForbiddenChar()}* удален";
            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter(Commands.MY_WISH_LISTS);
            
            await telegramSender.EditMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else
        {
            await telegramSender.ShowAlertAsync(BaseMessages.COMMAND_NOT_RECOGNIZED, cancellationToken);
        }
    }
}