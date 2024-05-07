using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Storage.Storages.Presents;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyPresents.MyPresentDeleteRequest;

public class MyPresentDeleteRequestUseCase(
    ISender sender,
    IPresentStorage presentStorage)
    : IRequestHandler<MyPresentDeleteRequestCommand>
{
    public async Task Handle(MyPresentDeleteRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;
        
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            var present = await presentStorage.GetPresent(presentId, cancellationToken);
            if(present == null) return;
            
            var textMessage = $"Удалить запись *{present.Name.MarkForbiddenChar()}*\\?";
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [InlineKeyboardButton.WithCallbackData(
                        "Да", $"{Commands.MY_PRESENT_DELETE}<?>{present.Id}"),
                ]
            ];
            keyboard = keyboard.AddBaseFooter($"{Commands.MY_PRESENT_INFO}<?>{present.Id}");
            
            var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
            var messageId = request.Param.CallbackQuery.Message?.MessageId;
            if (!(chatId.HasValue && messageId.HasValue)) return;
            await sender.EditMessageTextAsync(
                chatId: chatId.Value,
                messageId: messageId.Value,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}