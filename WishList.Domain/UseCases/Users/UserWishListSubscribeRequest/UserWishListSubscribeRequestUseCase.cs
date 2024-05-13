using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.Exceptions;
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
        if (command.Length < 2)
            throw new DomainException(BaseMessages.COMMAND_NOT_RECOGNIZED);

        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if (wishList == null)
                throw new DomainException(BaseMessages.WISH_LIST_NOT_FOUND);

            if (request.Param.User.SubscribeWishLists.Any(wl => wl.Id == wishList.Id))
            {
                await telegramSender.ShowAlertAsync($"Вы уже подписаны на {wishList.Name}", cancellationToken);
            }
            else
            {
                var foundUser = await userStorage.GetUser(wishList.AuthorId, cancellationToken);
                if (foundUser == null)
                    throw new DomainException(BaseMessages.USER_NOT_FOUND);

                var textMessage =
                    $"Подписаться на список *{wishList.Name.MarkForbiddenChar()}* пользователя {foundUser.ToString().MarkForbiddenChar()} \\?";

                List<List<InlineKeyboardButton>> keyboard =
                [
                    [
                        InlineKeyboardButton.WithCallbackData(
                            "👌 Да", $"{Commands.USERS_WISH_LIST_SUBSCRIBE}<?>{wishList.Id}"),
                    ],
                ];

                keyboard = keyboard.AddBaseFooter($"{Commands.USERS_WISH_LISTS_FIND_INFO}<?>{foundUser.Id}");

                await telegramSender.EditMessageAsync(
                    text: textMessage,
                    replyMarkup: new InlineKeyboardMarkup(keyboard),
                    cancellationToken: cancellationToken);
            }
        }
        else
        {
            throw new DomainException(BaseMessages.COMMAND_NOT_RECOGNIZED);
        }
    }
}