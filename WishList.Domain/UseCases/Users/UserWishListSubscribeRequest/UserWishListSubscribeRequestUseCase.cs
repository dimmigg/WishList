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
        if (request.Param.Commands.Length < 2)
            throw new DomainException(BaseMessages.CommandNotRecognized);

        if (int.TryParse(request.Param.Commands[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if (wishList is null)
                throw new DomainException(BaseMessages.WishListNotFound);

            if (request.Param.User.SubscribeWishLists.Any(wl => wl.Id == wishList.Id))
            {
                await telegramSender.ShowAlertAsync($"Вы уже подписаны на {wishList.Name}", cancellationToken);
            }
            else
            {
                var foundUser = await userStorage.GetUser(wishList.AuthorId, cancellationToken);
                if (foundUser is null)
                    throw new DomainException(BaseMessages.UserNotFound);

                var textMessage =
                    $"Подписаться на список *{wishList.Name.MarkForbiddenChar()}* пользователя {foundUser.ToString().MarkForbiddenChar()} \\?";

                List<List<InlineKeyboardButton>> keyboard =
                [
                    [
                        InlineKeyboardButton.WithCallbackData(
                            "👌 Да", $"{Commands.UsersWishListSubscribe}<?>{wishList.Id}"),
                    ],
                ];

                keyboard.AddBaseFooter($"{Commands.UsersWishListsFindInfo}<?>{foundUser.Id}");

                await telegramSender.EditMessageAsync(
                    text: textMessage,
                    replyMarkup: new InlineKeyboardMarkup(keyboard),
                    cancellationToken: cancellationToken);
            }
        }
        else
        {
            throw new DomainException(BaseMessages.CommandNotRecognized);
        }
    }
}