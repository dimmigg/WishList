using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;

namespace System;

public static class InlineKeyboardButtonExtensions
{
    public static List<List<InlineKeyboardButton>> AddBaseFooter(this List<List<InlineKeyboardButton>> keyboard,
        string? backCommand = null)
    {
        var backButton = InlineKeyboardButton.WithCallbackData("👈 Назад", backCommand ?? string.Empty);
        var homeButton = InlineKeyboardButton.WithCallbackData("🖖 Главное меню", Commands.MAIN);
        
        if (!string.IsNullOrWhiteSpace(backCommand))
            keyboard.Add([backButton, homeButton]);
        else
            keyboard.Add([homeButton]);

        return keyboard;
    }

    public static List<List<InlineKeyboardButton>> AddSelfDeleteButton(this List<List<InlineKeyboardButton>> keyboard)
    {
        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "❌ Отмена", Commands.SELF_DELETE_BUTTON)
        ]);

        return keyboard;
    }
}