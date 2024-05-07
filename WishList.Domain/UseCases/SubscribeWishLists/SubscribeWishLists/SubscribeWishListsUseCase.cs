using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.SubscribeWishLists.SubscribeWishLists;

public class SubscribeWishListsUseCase(
    ISender sender,
    IWishListStorage wishListStorage)
    : IRequestHandler<SubscribeWishListsCommand>
{
    public async Task Handle(SubscribeWishListsCommand request, CancellationToken cancellationToken)
    {
        if(request.Param.CallbackQuery == null) return;
        
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

        var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
        var messageId = request.Param.CallbackQuery.Message?.MessageId;
        if(!(chatId.HasValue && messageId.HasValue)) return;
        await sender.EditMessageTextAsync(
            chatId: chatId.Value,
            messageId: messageId.Value,
            text: sb.ToString(),
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}