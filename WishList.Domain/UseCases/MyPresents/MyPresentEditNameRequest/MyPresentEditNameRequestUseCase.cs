using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.UseCases.MyPresents.MyPresentEditNameRequest;

public class MyPresentEditNameRequestUseCase(
    ITelegramSender telegramSender,
    IUpdateUserUseCase updateUserUseCase)
    : IRequestHandler<MyPresentEditNameRequestCommand>
{
    public async Task Handle(MyPresentEditNameRequestCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            const string textMessage = "Введите название записи";

            updateUserUseCase.UpdateLastCommandUser(request.Param.User.Id, $"{Commands.PresentEditName}<?>{presentId}");
            
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