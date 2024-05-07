using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyPresents.MyPresents;

public class MyPresentsUseCase(
    ISender sender,
    IWishListStorage wishListStorage,
    IPresentStorage presentStorage)
    : IRequestHandler<MyPresentsCommand>
{
    public async Task Handle(MyPresentsCommand request, CancellationToken cancellationToken)
    {
        if(request.Param.CallbackQuery == null) return;

        var command = request.Param.Command.Split("<?>");
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
                sb.AppendLine($"Ваши записи из списка *{wishList.Name.MarkForbiddenChar()}*:");
                keyboard = presents
                    .Select(present => new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(present.Name, $"{Commands.MY_PRESENT_INFO}<?>{present.Id}"),
                    }).ToList();
            }
            else
            {
                sb.AppendLine($"Список *{wishList.Name.MarkForbiddenChar()}* пуст"); 
            }
        
            keyboard.Add([
                InlineKeyboardButton.WithCallbackData(
                    "+ Добавить", $"{Commands.MY_PRESENT_ADD_REQUEST}<?>{wishList.Id}")
            ]);
            keyboard = keyboard.AddBaseFooter($"{Commands.MY_WISH_LIST_INFO}<?>{wishList.Id}");

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
}
