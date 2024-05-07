using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Storage.Storages.Users;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.Users.UsersFindRequest;

public class UsersFindRequestUseCase(
    ISender sender,
    IUserStorage userStorage)
    : IRequestHandler<UsersFindRequestCommand>
{
    public async Task Handle(UsersFindRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

        const string textMessage = "Введи идентификатор или логин пользователя, чей список нужно добавить";

        await userStorage.UpdateLastCommandUser(request.Param.User.Id, Commands.USERS_FIND, cancellationToken);

        var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter();

        var chatId = request.Param.CallbackQuery.Message?.Chat.Id;
        var messageId = request.Param.CallbackQuery.Message?.MessageId;
        if (!(chatId.HasValue && messageId.HasValue)) return;
        
        await sender.EditMessageTextAsync(
            chatId: chatId.Value,
            messageId: messageId.Value,
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}