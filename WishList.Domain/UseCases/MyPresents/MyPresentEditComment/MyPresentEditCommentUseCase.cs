using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.Users;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyPresents.MyPresentEditComment;

public class MyPresentEditCommentUseCase(
    ISender sender,
    IPresentStorage presentStorage,
    IUserStorage userStorage
    )
    : IRequestHandler<MyPresentEditCommentCommand>
{

    public async Task Handle(MyPresentEditCommentCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Message == null || string.IsNullOrWhiteSpace(request.Param.Message.Text)) return;
        
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            const string textMessage = "Комментарий изменен";

            await userStorage.UpdateLastCommandUser(request.Param.User.Id, null, cancellationToken);
            var present = await presentStorage.UpdateComment(request.Param.Message.Text, presentId, cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter($"{Commands.MY_PRESENT_INFO}<?>{presentId}");

            await sender.SendTextMessageAsync(
                chatId: request.Param.Message.Chat.Id,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}