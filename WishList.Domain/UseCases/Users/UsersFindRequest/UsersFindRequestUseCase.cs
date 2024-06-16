using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.UseCases.Users.UsersFindRequest;

public class UsersFindRequestUseCase(
    ITelegramSender telegramSender,
    IUpdateUserUseCase updateUserUseCase
    ) : IRequestHandler<UsersFindRequestCommand>
{
    public async Task Handle(UsersFindRequestCommand request, CancellationToken cancellationToken)
    {
        const string textMessage = "Введите идентификатор или логин пользователя, чей список нужно добавить";

        updateUserUseCase.UpdateLastCommandUser(request.Param.User.Id, Commands.UsersFind);

        var keyboard = new List<List<InlineKeyboardButton>>();
        keyboard.AddBaseFooter(Commands.SubscribeUsers);

        await telegramSender.EditMessageAsync(
            text: textMessage,
            replyMarkup: new InlineKeyboardMarkup(keyboard),
            cancellationToken: cancellationToken);
    }
}