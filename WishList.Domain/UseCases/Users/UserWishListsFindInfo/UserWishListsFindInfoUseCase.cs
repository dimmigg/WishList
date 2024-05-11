using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Users.UserWishListsFindInfo;

public class UserWishListsFindInfoUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage)
    : IRequestHandler<UserWishListsFindInfoCommand>
{
    public async Task Handle(UserWishListsFindInfoCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        
        if (command.Length < 2)
            throw new DomainException(BaseMessages.COMMAND_NOT_RECOGNIZED);
        
        if (long.TryParse(command[1], out var userId))
        {
            var wishLists = await wishListStorage.GetWishLists(userId, cancellationToken);
            var user = await userStorage.GetUser(userId, cancellationToken);
            var sb = new StringBuilder();
            
            List<List<InlineKeyboardButton>> keyboard = [];
            
            if (wishLists.Length == 0)
            {
                sb.AppendLine($"У пользователя {user?.ToString()} нет списков");
            }
            else
            {
                sb.AppendLine($"Списки пользователя {user?.ToString()}\\:");
                keyboard = wishLists
                    .Select(wishList => new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(wishList.Name, $"{Commands.USERS_WISH_LIST_SUBSCRIBE_REQUEST}<?>{wishList.Id}"),
                    }).ToList();
            }

            keyboard = keyboard.AddBaseFooter();

            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else
        {
            throw new DomainException(BaseMessages.COMMAND_NOT_RECOGNIZED);
        }
    }
}