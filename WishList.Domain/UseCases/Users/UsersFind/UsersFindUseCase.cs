using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Users;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.Users.UsersFind;

public class UsersFindUseCase(
    ISender sender,
    IUserStorage userStorage)
    : IRequestHandler<UsersFindCommand>
{
    public async Task Handle(UsersFindCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Message == null || string.IsNullOrWhiteSpace(request.Param.Message.Text)) return;
        
        var users = await userStorage.FindUsers(request.Param.Message.Text, cancellationToken);
        var sb = new StringBuilder();
        List<List<InlineKeyboardButton>> keyboard = [];
        if (users == null || users.Length == 0)
        {
            sb.AppendLine("Пользователи не найдены");
        }
        else
        {
            sb.AppendLine($"Найдены пользователи:");
            keyboard = users
                .Select(user => new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(user.ToString().Replace("\\",""), $"{Commands.USERS_WISH_LISTS_FIND_INFO}<?>{user.Id}"),
                }).ToList();
        }

        keyboard = keyboard.AddBaseFooter();
        
        await sender.SendTextMessageAsync(
            chatId: request.Param.Message.Chat.Id,
            text: sb.ToString(),
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}