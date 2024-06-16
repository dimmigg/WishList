using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;

namespace System;

public static class InlineKeyboardButtonExtensions
{
    public static void AddBaseFooter(this List<List<InlineKeyboardButton>> keyboard,
        string? backCommand = null)
    {
        var backButton = InlineKeyboardButton.WithCallbackData("👈 Назад", backCommand ?? string.Empty);
        var homeButton = InlineKeyboardButton.WithCallbackData("🖖 Главное меню", Commands.Main);

        keyboard.Add(!string.IsNullOrWhiteSpace(backCommand)
            ? [backButton, homeButton]
            : [homeButton]);
    }

    public static void AddSelfDeleteButton(this List<List<InlineKeyboardButton>> keyboard)
    {
        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "❌ Отмена", Commands.SelfDeleteButton)
        ]);
    }
}