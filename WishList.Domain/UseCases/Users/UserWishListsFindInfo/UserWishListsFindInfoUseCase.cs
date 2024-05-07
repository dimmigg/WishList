using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.Users.UserWishListsFindInfo;

public class UserWishListsFindInfoUseCase(
    ISender sender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage)
    : IRequestHandler<UserWishListsFindInfoCommand>
{
    public async Task Handle(UserWishListsFindInfoCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (long.TryParse(command[1], out var userId))
        {
            var wishLists = await wishListStorage.GetWishLists(userId, cancellationToken);
            var user = await userStorage.GetUser(userId, cancellationToken);
            var sb = new StringBuilder();
            
            await userStorage.UpdateLastCommandUser(request.Param.User.Id, null, cancellationToken);
            List<List<InlineKeyboardButton>> keyboard = [];
            
            if (wishLists.Length == 0)
            {
                sb.AppendLine($"У пользователя {user?.ToString().MarkForbiddenChar()} нет списков");
            }
            else
            {
                sb.AppendLine($"Списки пользователя {user?.ToString().MarkForbiddenChar()}\\:");
                keyboard = wishLists
                    .Select(wishList => new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(wishList.Name, $"{Commands.USERS_WISH_LIST_SUBSCRIBE_REQUEST}<?>{wishList.Id}"),
                    }).ToList();
            }

            keyboard = keyboard.AddBaseFooter();

            var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
            var messageId = request.Param.CallbackQuery.Message?.MessageId;
            if (!(chatId.HasValue && messageId.HasValue)) return;
            await sender.EditMessageTextAsync(
                chatId: chatId.Value,
                messageId: messageId.Value,
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}