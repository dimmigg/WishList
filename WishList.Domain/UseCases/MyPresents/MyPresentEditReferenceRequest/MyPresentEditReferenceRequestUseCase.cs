using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.UseCases.MyPresents.MyPresentEditReferenceRequest;

public class MyPresentEditReferenceRequestUseCase(
    ITelegramSender telegramSender,
    IUpdateUserUseCase updateUserUseCase)
    : IRequestHandler<MyPresentEditReferenceRequestCommand>
{
    public async Task Handle(MyPresentEditReferenceRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Commands.Length < 2) return;
        if (int.TryParse(request.Param.Commands[1], out var presentId))
        {
            const string textMessage = "Введите ссылку записи";

            updateUserUseCase.UpdateLastCommandUser(request.Param.User.Id, $"{Commands.PresentEditReference}<?>{presentId}");
            
            var keyboard = new List<List<InlineKeyboardButton>>();
            keyboard.AddSelfDeleteButton();
            
            await telegramSender.AnswerCallbackQueryAsync(
                text: textMessage,
                cancellationToken: cancellationToken);
            
            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}