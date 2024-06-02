using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.MyPresents.MyPresentDeleteRequest;

public class MyPresentDeleteRequestUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage)
    : IRequestHandler<MyPresentDeleteRequestCommand>
{
    public async Task Handle(MyPresentDeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            var present = await presentStorage.GetPresent(presentId, cancellationToken);
            if(present is null) return;
            
            var textMessage = $"Удалить запись *{present.Name.MarkForbiddenChar()}*\\?";
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [InlineKeyboardButton.WithCallbackData(
                        "👌 Да", $"{Commands.MY_PRESENT_DELETE}<?>{present.Id}"),
                ]
            ];
            keyboard = keyboard.AddBaseFooter($"{Commands.MY_PRESENT_INFO}<?>{present.Id}");
            
            await telegramSender.EditMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}