﻿using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Users;

public class UserWishListsFindInfoUseCase(
    UseCaseParam param,
    ISender sender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage)
    : IUseCase
{
    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.CallbackQuery == null) return;

        var command = param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (long.TryParse(command[1], out var userId))
        {
            var wishLists = await wishListStorage.GetWishLists(userId, cancellationToken);
            var user = await userStorage.GetUser(userId, cancellationToken);
            var sb = new StringBuilder();
            
            await userStorage.UpdateLastCommandUser(param.User.Id, null, cancellationToken);
            List<List<InlineKeyboardButton>> keyboard = [];
            
            if (wishLists.Length == 0)
            {
                sb.AppendLine($"У пользователя {user?.ToString().MarkForbiddenChar()} нет списков");
            }
            else
            {
                sb.AppendLine($"Списки пользователя {user?.ToString().MarkForbiddenChar()}\\:");
                keyboard = wishLists
                    .Select(wishList => new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(wishList.Name, $"uwlsr<?>{wishList.Id}"),
                    }).ToList();
            }
 
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