using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;

namespace WishList.Domain.UseCases.Main.Main;

public class MainUseCase(
    ITelegramSender telegramSender) : IRequestHandler<MainCommand>
{
    public async Task Handle(MainCommand request, CancellationToken cancellationToken)
    {
        List<List<InlineKeyboardButton>> keyboard =
        [
            [
                InlineKeyboardButton.WithCallbackData(
                    "📝 Мои списки", Commands.MY_WISH_LISTS)
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    "🤍 Список друзей", Commands.SUBSCRIBE_WISH_LISTS)
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    "🔍 Поиск списка", Commands.USERS_FIND_REQUEST)
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    "❓ Как меня найти?", Commands.HOW_TO_FIND_ME)
            ]
        ];

        const string textMessage = "Я помогу узнать, что хотят получить твои друзья\\!\nА им расскажу, что хочешь получить ты\\!";

        if (request.Param.Message != null)
        {
            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else if(request.Param.CallbackQuery != null)
        {
            await telegramSender.EditMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}
