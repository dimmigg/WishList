using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;

namespace WishList.Domain.UseCases.Main.HowToFindMe;

public class HowToFindMeUseCase(
    ITelegramSender telegramSender) : IRequestHandler<HowToFindMeCommand>
{
    public async Task Handle(HowToFindMeCommand request, CancellationToken cancellationToken)
    {
        var keyboard = new List<List<InlineKeyboardButton>>();
        keyboard.AddBaseFooter();

        var textMessage = $"Логин: `{request.Param.User.Username}`\n" +
                          $"Идентификатор: `{request.Param.User.Id}`";
        
        await telegramSender.EditMessageAsync(
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}