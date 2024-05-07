using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.SubscribeWishLists.UnsubscribeWishList;

public class UnsubscribeWishListUseCase(
    ISender sender,
    IWishListStorage wishListStorage)
    : IRequestHandler<UnsubscribeWishListCommand>
{
    public async Task Handle(UnsubscribeWishListCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if(wishList == null) return;
            
            await wishListStorage.UnsubscribeWishList(request.Param.User.Id, wishListId, cancellationToken);
            var sb = new StringBuilder();
            sb.AppendLine($"Список *{wishList.Name.MarkForbiddenChar()}* удалён из избранного");

            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter(Commands.SUBSCRIBE_WISH_LISTS);

            var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
            var messageId = request.Param.CallbackQuery.Message?.MessageId;
            if (!(chatId.HasValue && messageId.HasValue)) return;
            await sender.EditMessageTextAsync(
                chatId: chatId.Value,
                messageId: messageId.Value,
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}