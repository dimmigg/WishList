using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases;

public class GetWishListsUseCase(
    BuildParam param,
    ISender sender,
    IWishListStorage wishListStorage)
    : IUseCase
{

    public async Task Execute(CancellationToken cancellationToken)
    {
        if(param.CallbackQuery == null) return;
        
        var sb = new StringBuilder("*Управление списками*\n");
        List<List<InlineKeyboardButton>> keyboard = [];
        var wishLists = await wishListStorage.GetWishLists(param.User.Id, cancellationToken);
        if (wishLists.Length != 0)
        {
            sb.AppendLine("Ваши списки:");
            keyboard = wishLists
                .Select(wishList => new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(wishList.Name, $"my-wish-lists-item<?>{wishList.Id}"),
                }).ToList();
            
        }
        
        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "+ Добавить", $"my-wish-lists-item-add")
        ]);
        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "<= Назад", "main")
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