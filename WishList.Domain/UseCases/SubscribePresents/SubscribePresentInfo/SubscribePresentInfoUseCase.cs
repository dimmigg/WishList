using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.SubscribePresents.SubscribePresentInfo;

public class SubscribePresentInfoUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage)
    : IRequestHandler<SubscribePresentInfoCommand>
{
    public async Task Handle(SubscribePresentInfoCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            var present = await presentStorage.GetPresent(presentId, cancellationToken);
            if(present == null) return;

            var keyboard = new List<List<InlineKeyboardButton>>();
            
            var sb = new StringBuilder();
            sb.AppendLine($"Подарок: *{present.Name.MarkForbiddenChar()}*");
            
            if(!string.IsNullOrWhiteSpace(present.Reference))
                sb.AppendLine($"Ссылка: *[тык]({present.Reference.MarkForbiddenChar()})*");
            
            if(!string.IsNullOrWhiteSpace(present.Comment))
                sb.AppendLine($"Комментарий: *{present.Comment?.MarkForbiddenChar()}*");
            if (present.ReserveForUserId.HasValue)
            {
                sb.AppendLine("*Подарок зарезервирован*");
                var fromReserve = command.Length == 3 ? "<?>r" : "";
                if(present.ReserveForUserId.Value == request.Param.User.Id)
                    keyboard.Add([
                        InlineKeyboardButton.WithCallbackData(
                            "⭕️ Убрать из резерва", $"{Commands.REMOVE_RESERVE_PRESENT}<?>{present.Id}{fromReserve}")
                    ]);
            }
            else
            {
                keyboard.Add([
                    InlineKeyboardButton.WithCallbackData(
                        "📌 Зарезервировать", $"{Commands.RESERVE_PRESENT}<?>{present.Id}<?>{request.Param.User.Id}")
                ]);
            }

            keyboard = keyboard.AddBaseFooter(command.Length == 3 
                ? $"{Commands.SUBSCRIBE_PRESENTS}<?>{present.WishListId}<?>{Commands.RESERVED}" 
                : $"{Commands.SUBSCRIBE_PRESENTS}<?>{present.WishListId}");
            
            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}