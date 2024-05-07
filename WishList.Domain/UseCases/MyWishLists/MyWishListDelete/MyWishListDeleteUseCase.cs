using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListDelete;

public class MyWishListDeleteUseCase(
    ISender sender,
    IWishListStorage wishListStorage)
    : IRequestHandler<MyWishListDeleteCommand>
{

    public async Task Handle(MyWishListDeleteCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if(wishList == null) return;
            var textMessage = $"Список *{wishList.Name.MarkForbiddenChar()}* удален";

            await wishListStorage.Delete(wishListId, cancellationToken);

            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter(Commands.MY_WISH_LISTS);
            
            var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
            var messageId = request.Param.CallbackQuery.Message?.MessageId;
            if (!(chatId.HasValue && messageId.HasValue)) return;
            await sender.EditMessageTextAsync(
                chatId: chatId.Value,
                messageId: messageId.Value,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}