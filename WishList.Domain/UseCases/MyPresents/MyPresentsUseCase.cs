using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyPresents;

public class MyPresentsUseCase(
    UseCaseParam param,
    ISender sender,
    IWishListStorage wishListStorage,
    IPresentStorage presentStorage)
    : IUseCase
{

    public async Task Execute(CancellationToken cancellationToken)
    {
        if(param.CallbackQuery == null) return;
        var commands = param.Command.Split("</>");
        var lastCommand = commands[^1];
        var command = lastCommand.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if (wishList == null) return;

            var sb = new StringBuilder();
            List<List<InlineKeyboardButton>> keyboard = [];
        var presents = await presentStorage.GetPresents(wishListId, cancellationToken);
        if (presents.Length != 0)
        {
            sb.AppendLine($"Ваши записи из списка *{wishList.Name}*:");
            keyboard = presents
                .Select(present => new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(present.Name, $"my-present-info<?>{present.Id}"),
                }).ToList();
            
        }
        else
        {
            sb.AppendLine($"Список *{wishList.Name}* пуст"); 
        }
        
        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "+ Добавить", $"my-present-add-request<?>{wishList.Id}")
        ]);
        keyboard.Add([
            
            InlineKeyboardButton.WithCallbackData(
            "« Назад", $"my-wish-list-info<?>{wishList.Id}"),
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
}
