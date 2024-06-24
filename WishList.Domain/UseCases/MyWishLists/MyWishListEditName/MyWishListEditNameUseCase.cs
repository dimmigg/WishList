using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListEditName;

public class MyWishListEditNameUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUpdateUserUseCase updateUserUseCase
    )
    : IRequestHandler<MyWishListEditNameCommand>
{
    public async Task Handle(MyWishListEditNameCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Commands.Length < 2)
            throw new DomainException(BaseMessages.CommandNotRecognized);
        
        if (int.TryParse(request.Param.Commands[1], out var wishListId))
        {
            updateUserUseCase.UpdateLastCommandUser(request.Param.User.Id);
            await wishListStorage.EditName(request.Param.Message!.Text!, wishListId, cancellationToken);
            
            const string textMessage = @"Отлично\! Название списка обновлено\!";
            var keyboard = new List<List<InlineKeyboardButton>>();
            keyboard.AddBaseFooter($"{Commands.WishListInfo}<?>{wishListId}");

            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else
        {
            throw new DomainException(BaseMessages.CommandNotRecognized);
        }
    }
}