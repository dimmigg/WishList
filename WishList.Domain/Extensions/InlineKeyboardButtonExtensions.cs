using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.UseCases;

namespace System;

public static class InlineKeyboardButtonExtensions
{
    public static List<List<InlineKeyboardButton>> AddBaseFooter(this List<List<InlineKeyboardButton>> keyboard,
        string? backCommand = null)
    {
        if (!string.IsNullOrWhiteSpace(backCommand))
            keyboard.Add([
                InlineKeyboardButton.WithCallbackData(
                    "« Назад", backCommand)
            ]);
        
        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "« Главное меню", Commands.MAIN)
        ]);

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