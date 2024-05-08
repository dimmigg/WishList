using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListEditName;

public class MyWishListEditNameUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage
    )
    : IRequestHandler<MyWishListEditNameCommand>
{
    public async Task Handle(MyWishListEditNameCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            const string textMessage = @"Отлично\! Название списка обновлено\!";

            await userStorage.UpdateLastCommandUser(request.Param.User.Id, null, cancellationToken);
            await wishListStorage.EditNameWishList(request.Param.Message.Text, wishListId, cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter($"{Commands.MY_WISH_LIST_INFO}<?>{wishListId}");

            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}