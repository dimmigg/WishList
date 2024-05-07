using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.Users;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyPresents.MyPresentEditReference;

public class MyPresentEditReferenceUseCase(
    ISender sender,
    IPresentStorage presentStorage,
    IUserStorage userStorage
    )
    : IRequestHandler<MyPresentEditReferenceCommand>
{
    public async Task Handle(MyPresentEditReferenceCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Message == null || string.IsNullOrWhiteSpace(request.Param.Message.Text)) return;
        
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            const string textMessage = "Ссылка изменена";

            await userStorage.UpdateLastCommandUser(request.Param.User.Id, null, cancellationToken);
            var present = await presentStorage.UpdateReference(request.Param.Message.Text, presentId, cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter($"{Commands.MY_PRESENT_INFO}<?>{present.Id}");

            await sender.SendTextMessageAsync(
                chatId: request.Param.Message.Chat.Id,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}