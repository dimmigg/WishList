using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.MyPresents.MyPresentAdd;

public class MyPresentAddUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage,
    IUserStorage userStorage
    )
    : IRequestHandler<MyPresentAddCommand>
{
    public async Task Handle(MyPresentAddCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            const string textMessage = "Отлично\\! Запись добавлена";

            await userStorage.UpdateLastCommandUser(request.Param.User.Id, null, cancellationToken);
            await presentStorage.AddPresent(request.Param.Message!.Text!, wishListId, cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter($"{Commands.MY_PRESENTS}<?>{wishListId}");

            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}