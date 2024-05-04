﻿using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.SubscribePresents;

public class SubscribePresentInfoUseCase(
    UseCaseParam param,
    ISender sender,
    IPresentStorage presentStorage)
    : IUseCase
{

    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.CallbackQuery == null) return;

        var command = param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            var present = await presentStorage.GetPresent(presentId, cancellationToken);
            if(present == null) return;

            var keyboard = new List<List<InlineKeyboardButton>>();
            
            var sb = new StringBuilder();
            sb.AppendLine($"Подарок: *{present.Name.MarkForbiddenChar()}*");
            sb.AppendLine($"Ссылка: *{present.Reference?.MarkForbiddenChar()}*");
            sb.AppendLine($"Комментарий: *{present.Comment?.MarkForbiddenChar()}*");
            if (present.ReserveForUserId.HasValue)
            {
                sb.AppendLine("*Подарок зарезервирован*");
                if(present.ReserveForUserId.Value == param.User.Id)
                    keyboard.Add([
                        InlineKeyboardButton.WithCallbackData(
                            "Убрать из резерва", $"remove-reserve-present<?>{present.Id}")
                    ]);
            }
            else
            {
                keyboard.Add([
                    InlineKeyboardButton.WithCallbackData(
                        "Зарезервировать", $"reserve-present<?>{present.Id}<?>{param.User.Id}")
                ]);
            }
            
            keyboard.Add([
                InlineKeyboardButton.WithCallbackData(
                    "« Назад", $"subscribe-presents<?>{present.WishListId}")
            ]);
            keyboard.Add([
                InlineKeyboardButton.WithCallbackData(
                    "« Главное меню", "main")
            ]);

            var chatId = param.CallbackQuery.Message?.Chat.Id;
            var messageId = param.CallbackQuery.Message?.MessageId;
            if (!(chatId.HasValue && messageId.HasValue)) return;
            await sender.EditMessageTextAsync(
                chatId: chatId.Value,
                messageId: messageId.Value,
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}