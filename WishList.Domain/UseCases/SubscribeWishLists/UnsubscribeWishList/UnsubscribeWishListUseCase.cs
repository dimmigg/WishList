using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribeWishLists.UnsubscribeWishList;

public class UnsubscribeWishListUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUpdateUserUseCase updateUser)
    : IRequestHandler<UnsubscribeWishListCommand>
{
    public async Task Handle(UnsubscribeWishListCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Commands.Length < 2) return;
        if (int.TryParse(request.Param.Commands[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if(wishList is null) return;
            
            await wishListStorage.UnsubscribeWishList(request.Param.User.Id, wishListId, cancellationToken);
            await updateUser.RefreshUser(request.Param.User.Id, cancellationToken);
            
            var sb = new StringBuilder();
            sb.AppendLine($"Список *{wishList.Name.MarkForbiddenChar()}* удалён из избранного");

            var keyboard = new List<List<InlineKeyboardButton>>();
            keyboard.AddBaseFooter(Commands.SubscribeUsers);

            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}