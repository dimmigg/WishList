using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Users.UserWishListSubscribe;

public class UserWishListSubscribeUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage)
    : IRequestHandler<UserWishListSubscribeCommand>
{
    public async Task Handle(UserWishListSubscribeCommand request, CancellationToken cancellationToken)
    {
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
            
            await telegramSender.EditMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}