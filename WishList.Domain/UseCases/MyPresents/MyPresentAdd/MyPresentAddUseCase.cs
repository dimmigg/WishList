using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.MyPresents.MyPresentAdd;

public class MyPresentAddUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage,
    IUpdateUserUseCase updateUserUseCase
    ) : IRequestHandler<MyPresentAddCommand>
{
    public async Task Handle(MyPresentAddCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            const string textMessage = "Отлично\\! Запись добавлена";

            updateUserUseCase.UpdateLastCommandUser(request.Param.User.Id);
            await presentStorage.AddPresent(request.Param.Message!.Text!, wishListId, cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>();
            keyboard.AddBaseFooter($"{Commands.Presents}<?>{wishListId}");

            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}