using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;

namespace WishList.Domain.UseCases;

public class StartUseCase(
    BuildParam param,
    ISender sender) : IUseCase
{
    public async Task Execute(Message message, RegisteredUser user, CancellationToken cancellationToken)
    {
        List<List<InlineKeyboardButton>> keyboard =
        [
            [
                InlineKeyboardButton.WithCallbackData(
                    "Мои списки", "my-wish-lists")
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    "Кому подарить", "subscritbe-wish-lists")
            ]
        ];

        const string usage = "*Главное меню*.\n" +
                             "Я помогу узнать, что хотят получить твои друзья! А также рассказать им, что хочешь получить ты!";

        await sender.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: usage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }

    public async Task Execute(CancellationToken cancellationToken)
    {
        List<List<InlineKeyboardButton>> keyboard =
        [
            [
                InlineKeyboardButton.WithCallbackData(
                    "Мои списки", "my-wish-lists")
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    "Кому подарить", "subscritbe-wish-lists")
            ]
        ];

        const string usage = "*Главное меню*.\n" +
                             "Я помогу узнать, что хотят получить твои друзья! А также рассказать им, что хочешь получить ты!";

        if (param.Message != null)
        {
            await sender.SendTextMessageAsync(
                chatId: param.Message.Chat.Id,
                text: usage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else if(param.CallbackQuery != null)
        {
            await sender.EditMessageTextAsync(
                chatId: param.CallbackQuery.Message.Chat.Id,
                messageId:param.CallbackQuery.Message.MessageId,
                text: usage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}
