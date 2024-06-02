using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.MyPresents.MyPresentDelete;

public class MyPresentDeleteUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage)
    : IRequestHandler<MyPresentDeleteCommand>
{
    public async Task Handle(MyPresentDeleteCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            const string textMessage = "Запись удалена";
            var present = await presentStorage.GetPresent(presentId, cancellationToken);
            if(present is null) return;

            await presentStorage.Delete(presentId, cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter($"{Commands.MY_PRESENTS}<?>{present.WishListId}");
            
            await telegramSender.EditMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}