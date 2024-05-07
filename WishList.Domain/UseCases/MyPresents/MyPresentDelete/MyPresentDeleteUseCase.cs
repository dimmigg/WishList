using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Presents;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyPresents.MyPresentDelete;

public class MyPresentDeleteUseCase(
    ISender sender,
    IPresentStorage presentStorage)
    : IRequestHandler<MyPresentDeleteCommand>
{
    public async Task Handle(MyPresentDeleteCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;
        
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            const string textMessage = "Запись удалена";
            var present = await presentStorage.GetPresent(presentId, cancellationToken);
            if(present == null) return;

            await presentStorage.Delete(presentId, cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter($"{Commands.MY_PRESENTS}<?>{present.WishListId}");

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