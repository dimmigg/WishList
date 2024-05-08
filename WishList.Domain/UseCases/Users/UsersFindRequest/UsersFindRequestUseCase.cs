using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.Users.UsersFindRequest;

public class UsersFindRequestUseCase(
    ITelegramSender telegramSender,
    IUserStorage userStorage)
    : IRequestHandler<UsersFindRequestCommand>
{
    public async Task Handle(UsersFindRequestCommand request, CancellationToken cancellationToken)
    {
        const string textMessage = "Введите идентификатор или логин пользователя, чей список нужно добавить";

        await userStorage.UpdateLastCommandUser(request.Param.User.Id, Commands.USERS_FIND, cancellationToken);

        var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter();

        await telegramSender.EditMessageAsync(
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}