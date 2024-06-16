using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListAdd;

public class MyWishListAddUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUpdateUserUseCase updateUserUseCase
    ) : IRequestHandler<MyWishListAddCommand>
{
    public async Task Handle(MyWishListAddCommand request, CancellationToken cancellationToken)
    {
        const string textMessage = "Отлично\\! Список добавлен";

        updateUserUseCase.UpdateLastCommandUser(request.Param.User.Id, null);
        await wishListStorage.AddWishList(request.Param.Message!.Text!, request.Param.User.Id, cancellationToken);

        var keyboard = new List<List<InlineKeyboardButton>>();
        keyboard.AddBaseFooter(Commands.WishLists);
        
        await telegramSender.SendMessageAsync(
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}