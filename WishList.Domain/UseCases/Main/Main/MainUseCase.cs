using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.Main.Main;

public class MainUseCase(
    ISender sender) : IRequestHandler<MainCommand>
{
    public async Task Handle(MainCommand request, CancellationToken cancellationToken)
    {
        if (request.Param == null || (request.Param.Message == null && request.Param.CallbackQuery == null))
            throw new DomainException("Параметы команды не валидны");
        
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
                    "❓ Как меня найти?", UseCases.Commands.HOW_TO_FIND_ME)
            ]
        ];

        const string textMessage = "Я помогу узнать, что хотят получить твои друзья\\!\nА им расскажу, что хочешь получить ты\\!";

        if (request.Param.Message != null)
        {
            await sender.SendTextMessageAsync(
                chatId: request.Param.Message.Chat.Id,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else if(request.Param.CallbackQuery != null)
        {
            await sender.EditMessageTextAsync(
                chatId: request.Param.CallbackQuery.Message.Chat.Id,
                messageId: request.Param.CallbackQuery.Message.MessageId,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}
