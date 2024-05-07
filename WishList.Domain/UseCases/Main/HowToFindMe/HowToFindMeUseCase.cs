using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Exceptions;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.Main.HowToFindMe;

public class HowToFindMeUseCase(
    ISender sender) : IRequestHandler<HowToFindMeCommand>
{
    public async Task Handle(HowToFindMeCommand request, CancellationToken cancellationToken)
    {
        if (request.Param == null || (request.Param.Message == null && request.Param.CallbackQuery == null))
            throw new DomainException("Параметы команды не валидны");

        var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter();

        var textMessage = $"Логин: `{request.Param.User.Username}`\n" +
                          $"Идентификатор: `{request.Param.User.Id}`";

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
                messageId:request.Param.CallbackQuery.Message.MessageId,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}
