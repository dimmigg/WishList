using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.MyPresents.MyPresentEditReference;

public class MyPresentEditReferenceUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage,
    IUpdateUserUseCase updateUserUseCase
    )
    : IRequestHandler<MyPresentEditReferenceCommand>
{
    public async Task Handle(MyPresentEditReferenceCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Commands.Length < 2) return;
        if (int.TryParse(request.Param.Commands[1], out var presentId))
        {
            const string textMessage = "Ссылка изменена";

            updateUserUseCase.UpdateLastCommandUser(request.Param.User.Id, null);
            await presentStorage.UpdateReference(request.Param.Message!.Text!, presentId, cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>();
            keyboard.AddBaseFooter($"{Commands.PresentInfo}<?>{presentId}");

            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}