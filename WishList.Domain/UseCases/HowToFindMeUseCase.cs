using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;

namespace WishList.Domain.UseCases;

public class HowToFindMeUseCase(
    UseCaseParam? param,
    ISender sender) : IUseCase
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param == null || (param?.Message == null && param?.CallbackQuery == null))
            throw new DomainException("Параметы команды не валидны");
        
        List<List<InlineKeyboardButton>> keyboard =
        [
            [
                InlineKeyboardButton.WithCallbackData(
                    "« Главное меню", "main")
            ]
        ];

        var textMessage = $"Логин: `{param.User.Username}`\n" +
                          $"Идентификатор: `{param.User.Id}`";

        if (param.Message != null)
        {
            await sender.SendTextMessageAsync(
                chatId: param.Message.Chat.Id,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else if(param.CallbackQuery != null)
        {
            await sender.EditMessageTextAsync(
                chatId: param.CallbackQuery.Message.Chat.Id,
                messageId:param.CallbackQuery.Message.MessageId,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}
