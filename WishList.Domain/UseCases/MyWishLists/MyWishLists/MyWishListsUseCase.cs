using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyWishLists.MyWishLists;

public class MyWishListsUseCase(
    ISender sender,
    IWishListStorage wishListStorage)
    : IRequestHandler<MyWishListsCommand>
{
    public async Task Handle(MyWishListsCommand request, CancellationToken cancellationToken)
    {
        if(request.Param.CallbackQuery == null) return;
        
        List<List<InlineKeyboardButton>> keyboard = [];
        var wishLists = await wishListStorage.GetWishLists(request.Param.User.Id, cancellationToken);
        var sb = new StringBuilder();
        if (wishLists.Length != 0)
        {
            sb.AppendLine("Ваши списки:");
            keyboard = wishLists
                .Select(wishList => new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData($"{wishList.Name} ({wishList.Presents.Count})", $"{Commands.MY_WISH_LIST_INFO}<?>{wishList.Id}"),
                }).ToList();
            
        }
        else
        {
            sb.AppendLine("🏖 Еще нет списков");
        }
        
        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "+ Добавить", Commands.MY_WISH_LIST_ADD_REQUEST)
        ]);
        keyboard.AddBaseFooter();

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