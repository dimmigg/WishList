using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribeWishLists.SubscribeWishLists;

public class SubscribeWishListsUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage)
    : IRequestHandler<SubscribeWishListsCommand>
{
    public async Task Handle(SubscribeWishListsCommand request, CancellationToken cancellationToken)
    {
        List<List<InlineKeyboardButton>> keyboard = [];
        var wishLists = (await wishListStorage.GetSubscribeWishLists(request.Param.User.Id, cancellationToken)).ToArray();
        var sb = new StringBuilder();
        if (wishLists.Length != 0)
        {
            sb.AppendLine("Ваши подписки:");
            keyboard = wishLists
                .Select(wishList => new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData($"{wishList.Name} ({wishList.Presents.Count})", $"{Commands.SUBSCRIBE_WISH_LIST_INFO}<?>{wishList.Id}"),
                }).ToList();
        }
        else
        {
            sb.AppendLine("Еще нет подписок");
        }

        keyboard = keyboard.AddBaseFooter();

        await telegramSender.EditMessageAsync(
            text: sb.ToString(),
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}