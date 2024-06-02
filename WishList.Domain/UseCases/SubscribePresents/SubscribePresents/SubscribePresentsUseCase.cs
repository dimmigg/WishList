using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribePresents.SubscribePresents;

public class SubscribePresentsUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage,
    IWishListStorage wishListStorage)
    : IRequestHandler<SubscribePresentsCommand>
{
    public async Task Handle(SubscribePresentsCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if(wishList is null) return;

            List<List<InlineKeyboardButton>> keyboard = [];
            var wishLists = await presentStorage.GetSubscribePresents(wishListId, cancellationToken);
            
            var sb = new StringBuilder();
            if (wishLists.Length != 0)
            {
                if (command.Length == 3)
                {
                    sb.AppendLine($"Мои резервы из списка *{wishList.Name.MarkForbiddenChar()}*:");
                    wishLists = wishLists.Where(p => p.ReserveForUserId == request.Param.User.Id).ToArray();
                    keyboard = wishLists
                        .Select(present =>
                        {
                            var isReserve = present.ReserveForUserId.HasValue
                                ? present.ReserveForUserId.Value == request.Param.User.Id ? "🟡 " : "🔴 "
                                : "🟢 ";
                            return new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData($"{isReserve}{present.Name}",
                                $"{Commands.SUBSCRIBE_PRESENT_INFO}<?>{present.Id}<?>{Commands.RESERVED}"),
                            };
                        }).ToList();
                }
                else
                {
                    sb.AppendLine($"Список *{wishList.Name.MarkForbiddenChar()}*:");
                    keyboard = wishLists
                        .Select(present =>
                        {
                            var isReserve = present.ReserveForUserId.HasValue
                                ? present.ReserveForUserId.Value == request.Param.User.Id ? "🟡 " : "🔴 "
                                : "🟢 ";
                            return new List<InlineKeyboardButton>
                            {
                                InlineKeyboardButton.WithCallbackData($"{isReserve}{present.Name}",
                                    $"{Commands.SUBSCRIBE_PRESENT_INFO}<?>{present.Id}"),
                            };
                        }).ToList();
                }
            }
            else
            {
                sb.AppendLine($"Список *{wishList.Name.MarkForbiddenChar()}* пуст");
            }

            keyboard = keyboard.AddBaseFooter($"{Commands.SUBSCRIBE_WISH_LIST_INFO}<?>{wishList.Id}");
            
            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}