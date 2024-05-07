using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.Users.UserWishListSubscribe;

public class UserWishListSubscribeUseCase(
    ISender sender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage)
    : IRequestHandler<UserWishListSubscribeCommand>
{
    public async Task Handle(UserWishListSubscribeCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if(wishList == null) return;
            var foundUser = await userStorage.GetUser(wishList.AuthorId, cancellationToken);
            if(foundUser == null) return;

            await userStorage.AddSubscribeWishList(request.Param.User.Id, wishListId, cancellationToken);
            
            var textMessage = $"Список *{wishList.Name}* добавлен в избранное";

            var keyboard =
                new List<List<InlineKeyboardButton>>().AddBaseFooter(
                    $"{Commands.USERS_WISH_LISTS_FIND_INFO}<?>{foundUser.Id}");
            
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