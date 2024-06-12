using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.Exceptions;
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
            throw new DomainException(BaseMessages.CommandNotRecognized);
        
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);

            if (wishList is null)
                throw new DomainException(BaseMessages.WishListNotFound);
            
            await wishListStorage.Delete(wishListId, cancellationToken);

            var textMessage = $"Список *{wishList.Name.MarkForbiddenChar()}* удален";
            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter(Commands.WishLists);
            
            await telegramSender.EditMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else
        {
            throw new DomainException(BaseMessages.CommandNotRecognized);
        }
    }
}