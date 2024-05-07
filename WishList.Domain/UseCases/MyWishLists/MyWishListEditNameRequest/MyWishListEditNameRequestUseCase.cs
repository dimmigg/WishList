using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListEditNameRequest;

public class MyWishListEditNameRequestUseCase(
    ISender sender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage)
    : IRequestHandler<MyWishListEditNameRequestCommand>
{
    public async Task Handle(MyWishListEditNameRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if (wishList == null) return;
            var textMessage = $"Введите новое название списка *`{wishList.Name.MarkForbiddenChar()}`*";
            var answerMessage = $"Введите новое название списка {wishList.Name.MarkForbiddenChar()}";
            await userStorage.UpdateLastCommandUser(request.Param.User.Id, $"{Commands.MY_WISH_LIST_EDIT_NAME}<?>{wishList.Id}", cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>().AddSelfDeleteButton();
            
            var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
            if (!chatId.HasValue) return;
            
            await sender.AnswerCallbackQueryAsync(
                callbackQueryId: request.Param.CallbackQuery.Id,
                text: answerMessage,
                cancellationToken: cancellationToken);
            
            await sender.SendTextMessageAsync(
                chatId: chatId.Value,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}