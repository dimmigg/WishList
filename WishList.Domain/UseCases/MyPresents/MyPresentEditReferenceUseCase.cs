﻿using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;
using WishList.Storage.Storages.Users;

namespace WishList.Domain.UseCases.MyPresents;

public class MyPresentEditReferenceUseCase(
    UseCaseParam param,
    ISender sender,
    IPresentStorage presentStorage,
    IUserStorage userStorage
    )
    : IUseCase
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.Message == null || string.IsNullOrWhiteSpace(param.Message.Text)) return;
        
        var command = param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            const string textMessage = "Ссылка изменена";

            await userStorage.UpdateLastCommandUser(param.User.Id, null, cancellationToken);
            var present = await presentStorage.UpdateReference(param.Message.Text, presentId, cancellationToken);

            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "« Назад", $"mpi<?>{present.Id}"),
                    InlineKeyboardButton.WithCallbackData(
                        "« Главное меню", "main")
                ]
            ];

            await sender.SendTextMessageAsync(
                chatId: param.Message.Chat.Id,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }

}