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
                    "📝 Мои списки", Commands.WishLists)
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    "🤍 Друзья", Commands.SubscribeUsers)
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    "❓ Как меня найти?", Commands.HowToFindMe)
            ]
        ];

        const string textMessage = "Я помогу узнать, что хотят получить твои друзья\\!\nА им расскажу, что хочешь получить ты\\!";

        if (request.Param.Message is not null)
        {
            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else if(request.Param.CallbackQuery is not null)
        {
            await telegramSender.EditMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}
