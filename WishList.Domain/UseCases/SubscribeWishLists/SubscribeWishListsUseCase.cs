using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribeWishLists;

public class SubscribeWishListsUseCase(
    UseCaseParam param,
    ISender sender,
    IWishListStorage wishListStorage)
    : IUseCase
{

    public async Task Execute(CancellationToken cancellationToken)
    {
        if(param.CallbackQuery == null) return;
        
        List<List<InlineKeyboardButton>> keyboard = [];
        var wishLists = (await wishListStorage.GetSubscribeWishLists(param.User.Id, cancellationToken)).ToArray();
        var sb = new StringBuilder();
        if (wishLists.Length != 0)
        {
            sb.AppendLine("Ваши подписки:");
            keyboard = wishLists
                .Select(wishList => new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData($"{wishList.Name} ({wishList.Presents.Count})", $"swli<?>{wishList.Id}"),
                }).ToList();
            
        }
        else
        {
            sb.AppendLine("Еще нет подписок");
        }
        
        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "« Главное меню", "main")
        ]);


        var chatId = param.CallbackQuery.Message?.Chat.Id;
        var messageId = param.CallbackQuery.Message?.MessageId;
        if(!(chatId.HasValue && messageId.HasValue)) return;
        await sender.EditMessageTextAsync(
            chatId: chatId.Value,
            messageId: messageId.Value,
            text: sb.ToString(),
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}