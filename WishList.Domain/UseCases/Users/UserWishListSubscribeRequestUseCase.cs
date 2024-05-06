﻿using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Users;

public class UserWishListSubscribeRequestUseCase(
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
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if(wishList == null) return;
            var user = await userStorage.GetUser(wishList.AuthorId, cancellationToken);
            if(user == null) return;
            
            var textMessage = $"Подписаться на список *{wishList.Name.MarkForbiddenChar()}* пользователя {user.ToString().MarkForbiddenChar()} \\?";
            
            List<List<InlineKeyboardButton>> keyboard =
            [
                [InlineKeyboardButton.WithCallbackData(
                        "Да", $"uwls<?>{wishList.Id}"),
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "« Назад", $"uwlf<?>{user.Id}"),
                    InlineKeyboardButton.WithCallbackData(
                        "« Главное меню", "main")
                ]
            ];
            
            var chatId = param.CallbackQuery.Message?.Chat.Id;
            var messageId = param.CallbackQuery.Message?.MessageId;
            if (!(chatId.HasValue && messageId.HasValue)) return;
            await sender.EditMessageTextAsync(
                chatId: chatId.Value,
                messageId: messageId.Value,
                text: textMessage,
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
    
}