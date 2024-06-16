using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.Users.UsersFind;

public class UsersFindUseCase(
    ITelegramSender telegramSender,
    IUserStorage userStorage)
    : IRequestHandler<UsersFindCommand>
{
    public async Task Handle(UsersFindCommand request, CancellationToken cancellationToken)
    {
        var messageText = request.Param.Message!.Text!;
        var sb = new StringBuilder();
        List<List<InlineKeyboardButton>> keyboard = [];
        if (messageText.Length < 5)
        {
            sb.AppendLine("Необходимо ввести не менее пяти символов");
        }
        else
        {
            var users = await userStorage.FindUsers(messageText, cancellationToken);
            if (users == null || users.Length == 0)
            {
                sb.AppendLine("Пользователи не найдены 🧐");
            }
            else
            {
                sb.AppendLine("Найдены пользователи:");
                keyboard = users
                    .Select(user => new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(user.ToString().Replace("\\", ""),
                            $"{Commands.UsersWishListsFindInfo}<?>{user.Id}"),
                    }).ToList();
            }
        }

        keyboard.AddBaseFooter();

        await telegramSender.SendMessageAsync(
            text: sb.ToString(),
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}