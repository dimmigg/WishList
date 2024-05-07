using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.Users.UserWishListSubscribeRequest;

public class UserWishListSubscribeRequestUseCase(
    ISender sender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage)
    : IRequestHandler<UserWishListSubscribeRequestCommand>
{
    public async Task Handle(UserWishListSubscribeRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

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
                        "Да", $"{Commands.USERS_WISH_LIST_SUBSCRIBE}<?>{wishList.Id}"),
                ],
            ];

            keyboard = keyboard.AddBaseFooter($"{Commands.USERS_WISH_LISTS_FIND_INFO}<?>{user.Id}");
            
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