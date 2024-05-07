using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListEditName;

public class MyWishListEditNameUseCase(
    ISender sender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage
    )
    : IRequestHandler<MyWishListEditNameCommand>
{
    public async Task Handle(MyWishListEditNameCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Message == null || string.IsNullOrWhiteSpace(request.Param.Message.Text)) return;

        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            const string textMessage = @"Отлично\! Название списка обновлено\!";

            await userStorage.UpdateLastCommandUser(request.Param.User.Id, null, cancellationToken);
            await wishListStorage.EditNameWishList(request.Param.Message.Text, wishListId, cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter($"{Commands.MY_WISH_LIST_INFO}<?>{wishListId}");

            await sender.SendTextMessageAsync(
                chatId: request.Param.Message.Chat.Id,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}