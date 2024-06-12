using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishLists;

public class MyWishListsUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage)
    : IRequestHandler<MyWishListsCommand>
{
    public async Task Handle(MyWishListsCommand request, CancellationToken cancellationToken)
    {
        List<List<InlineKeyboardButton>> keyboard = [];
        var wishLists = await wishListStorage.GetWishLists(request.Param.User.Id, cancellationToken);
        var sb = new StringBuilder();
        if (wishLists.Length != 0)
        {
            sb.AppendLine("Ваши списки:");
            keyboard = wishLists
                .Select(wishList => new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData($"{wishList.Name} ({wishList.Presents.Count})", $"{Commands.WishListInfo}<?>{wishList.Id}"),
                }).ToList();
        }
        else
        {
            sb.AppendLine("🏖 Еще нет списков");
        }
        
        keyboard.Add([
            InlineKeyboardButton.WithCallbackData(
                "✌️ Добавить", Commands.WishListAddRequest)
        ]);
        keyboard.AddBaseFooter();

        await telegramSender.EditMessageAsync(
            text: sb.ToString(),
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}