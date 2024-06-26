﻿using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.UpdateUser;

namespace WishList.Domain.UseCases.MyPresents.MyPresentAddRequest;

public class MyPresentAddRequestUseCase(
    ITelegramSender telegramSender,
    IUpdateUserUseCase updateUserUseCase
    ) : IRequestHandler<MyPresentAddRequestCommand>
{
    public async Task Handle(MyPresentAddRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Commands.Length < 2) return;
        if (int.TryParse(request.Param.Commands[1], out var wishListId))
        {
            const string textMessage = "Введите название новой записи";

            updateUserUseCase.UpdateLastCommandUser(request.Param.User.Id, $"{Commands.PresentAdd}<?>{wishListId}");
            
            var keyboard = new List<List<InlineKeyboardButton>>();
            keyboard.AddSelfDeleteButton();
            
            await telegramSender.AnswerCallbackQueryAsync(
                text: textMessage,
                cancellationToken: cancellationToken);
            
            await telegramSender.SendMessageAsync(
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}