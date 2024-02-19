using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.Users;

public class UsersFindUseCase(
    UseCaseParam param,
    ISender sender,
    IUserStorage userStorage)
    : IUseCase
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.Message == null || string.IsNullOrWhiteSpace(param.Message.Text)) return;
        await userStorage.UpdateLastCommandUser(param.User.Id, null, cancellationToken);
        var users = await userStorage.FindUsers(param.Message.Text, cancellationToken);
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
                    InlineKeyboardButton.WithCallbackData(user.ToString(), $"find-user-wish-lists<?>{user.Id}"),
                }).ToList();
        }

        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "« Главное меню", "main")
        ]);
        
        await sender.SendTextMessageAsync(
            chatId: param.Message.Chat.Id,
            text: sb.ToString(),
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}