using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListEditNameRequest;

public class MyWishListEditNameRequestUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage)
    : IRequestHandler<MyWishListEditNameRequestCommand>
{
    public async Task Handle(MyWishListEditNameRequestCommand request, CancellationToken cancellationToken)
    {
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
            
            await telegramSender.AnswerCallbackQueryAsync(
                text: answerMessage,
                cancellationToken: cancellationToken);
            
            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}