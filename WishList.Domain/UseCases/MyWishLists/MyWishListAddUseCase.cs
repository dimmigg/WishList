using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists;

public class MyWishListAddUseCase(
    UseCaseParam param,
    ISender sender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage
    )
    : IUseCase
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        if(param.Message == null || string.IsNullOrWhiteSpace(param.Message.Text)) return;
        
        const string textMessage = "Отлично\\! Список добавлен";

        await userStorage.UpdateLastCommandUser(param.User.Id, null, cancellationToken);
        await wishListStorage.AddWishList(param.Message.Text, param.User.Id, cancellationToken);
        
        List<List<InlineKeyboardButton>> keyboard =
        [
            [
                InlineKeyboardButton.WithCallbackData(
                    "« Назад", $"mwl"),
                InlineKeyboardButton.WithCallbackData(
                    "« Главное меню", "main")
            ]
        ];

        await sender.SendTextMessageAsync(
            chatId: param.Message.Chat.Id,
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
    
}