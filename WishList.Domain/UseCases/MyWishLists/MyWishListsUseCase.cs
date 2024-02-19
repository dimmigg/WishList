using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists;

public class MyWishListsUseCase(
    UseCaseParam param,
    ISender sender,
    IWishListStorage wishListStorage)
    : IUseCase
{

    public async Task Execute(CancellationToken cancellationToken)
    {
        if(param.CallbackQuery == null) return;
        
        List<List<InlineKeyboardButton>> keyboard = [];
        var wishLists = await wishListStorage.GetWishLists(param.User.Id, cancellationToken);
        var sb = new StringBuilder();
        if (wishLists.Length != 0)
        {
            sb.AppendLine("Ваши списки:");
            keyboard = wishLists
                .Select(wishList => new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData($"{wishList.Name} ({wishList.Presents.Count})", $"my-wish-list-info<?>{wishList.Id}"),
                }).ToList();
            
        }
        else
        {
            sb.AppendLine("Еще нет списков");
        }
        
        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "+ Добавить", $"my-wish-list-name-request")
        ]);
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