using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Users.UserWishListSubscribeRequest;

public class UserWishListSubscribeRequestUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage)
    : IRequestHandler<UserWishListSubscribeRequestCommand>
{
    public async Task Handle(UserWishListSubscribeRequestCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if(wishList == null) return;
            var user = await userStorage.GetUser(wishList.AuthorId, cancellationToken);
            if(user == null) return;
            
            var textMessage = $"Подписаться на список *{wishList.Name.MarkForbiddenChar()}* пользователя {user.ToString().MarkForbiddenChar()} \\?";
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [InlineKeyboardButton.WithCallbackData(
                        "👌 Да", $"{Commands.USERS_WISH_LIST_SUBSCRIBE}<?>{wishList.Id}"),
                ],
            ];

            keyboard = keyboard.AddBaseFooter($"{Commands.USERS_WISH_LISTS_FIND_INFO}<?>{user.Id}");
            
            await telegramSender.EditMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}