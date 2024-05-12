﻿using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.MyPresents.MyPresentEditComment;

public class MyPresentEditCommentUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage,
    IUserStorage userStorage
    )
    : IRequestHandler<MyPresentEditCommentCommand>
{

    public async Task Handle(MyPresentEditCommentCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            const string textMessage = "Комментарий изменен";

            await userStorage.UpdateLastCommandUser(request.Param.User.Id, null, cancellationToken);
            await presentStorage.UpdateComment(request.Param.Message.Text, presentId, cancellationToken);
            
            var keyboard = new List<List<InlineKeyboardButton>>().AddBaseFooter($"{Commands.MY_PRESENT_INFO}<?>{presentId}");

            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}