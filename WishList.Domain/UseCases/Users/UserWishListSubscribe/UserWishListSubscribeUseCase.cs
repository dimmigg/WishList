using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Users.UserWishListSubscribe;

public class UserWishListSubscribeUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage,
    IUpdateUserUseCase updateUser)
    : IRequestHandler<UserWishListSubscribeCommand>
{
    public async Task Handle(UserWishListSubscribeCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        
        if (command.Length < 2)
            throw new DomainException(BaseMessages.CommandNotRecognized);
        
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            
            if(wishList is null)
                throw new DomainException(BaseMessages.WishListNotFound);
            
            var foundUser = await userStorage.GetUser(wishList.AuthorId, cancellationToken);
            if(foundUser is null)
                throw new DomainException(BaseMessages.UserNotFound);

            await userStorage.AddSubscribeWishList(request.Param.User.Id, wishListId, cancellationToken);
            await updateUser.RefreshUser(request.Param.User.Id, cancellationToken);
            
            var textMessage = $"Список *{wishList.Name.MarkForbiddenChar()}* добавлен в избранное\\.";
            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "🧾 Список желаний", $"{Commands.SubscribePresents}<?>{wishListId}")
                ],
            ];
            keyboard.AddBaseFooter($"{Commands.UsersWishListsFindInfo}<?>{foundUser.Id}");
            
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