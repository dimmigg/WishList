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
        if (request.Param.Commands.Length < 2) return;
        if (int.TryParse(request.Param.Commands[1], out var presentId))
        {
            var present = await presentStorage.GetPresent(presentId, cancellationToken);
            if(present is null) return;

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
                var fromReserve = request.Param.Commands.Length == 3 ? "<?>r" : "";
                if(present.ReserveForUserId.Value == request.Param.User.Id)
                    keyboard.Add([
                        InlineKeyboardButton.WithCallbackData(
                            "⭕️ Убрать из резерва", $"{Commands.RemoveReservePresent}<?>{present.Id}{fromReserve}")
                    ]);
            }
            else
            {
                keyboard.Add([
                    InlineKeyboardButton.WithCallbackData(
                        "📌 Зарезервировать", $"{Commands.ReservePresent}<?>{present.Id}<?>{request.Param.User.Id}")
                ]);
            }

            keyboard.AddBaseFooter(request.Param.Commands.Length == 3 
                ? $"{Commands.SubscribePresents}<?>{present.WishListId}<?>{Commands.Reserved}" 
                : $"{Commands.SubscribePresents}<?>{present.WishListId}");
            
            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}