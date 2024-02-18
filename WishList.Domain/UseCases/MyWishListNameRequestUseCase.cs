using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases;

public class MyWishListNameRequestUseCase(
    UseCaseParam param,
    ISender sender,
    IUserStorage userStorage)
    : IUseCase
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        if(param.CallbackQuery == null) return;
        
        const string textMessage = "Введите название нового списка";

        await userStorage.UpdateLastCommandUser(param.User.Id, "my-wish-list-add", cancellationToken);

        var chatId = param.CallbackQuery.Message?.Chat.Id;
        if(!chatId.HasValue) return;
        await sender.SendTextMessageAsync(
            chatId: chatId.Value,
            text: textMessage,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }
    
}