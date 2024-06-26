﻿using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.Exceptions;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Users;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.Users.UserWishListsFindInfo;

public class UserWishListsFindInfoUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IUserStorage userStorage)
    : IRequestHandler<UserWishListsFindInfoCommand>
{
    public async Task Handle(UserWishListsFindInfoCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Commands.Length < 2)
            throw new DomainException(BaseMessages.CommandNotRecognized);
        
        if (long.TryParse(request.Param.Commands[1], out var userId))
        {
            var wishLists = await wishListStorage.GetWishLists(userId, cancellationToken);
            var user = await userStorage.GetUser(userId, cancellationToken);
            if(user is null)
                throw new DomainException(BaseMessages.UserNotFound);
            var sb = new StringBuilder();
            
            List<List<InlineKeyboardButton>> keyboard = [];
            
            if (wishLists.Length == 0)
            {
                sb.AppendLine($"У пользователя {user.ToString().MarkForbiddenChar()} нет списков");
            }
            else
            {
                sb.AppendLine($"Списки пользователя {user.ToString().MarkForbiddenChar()}\\:");
                keyboard = wishLists
                    .Select(wishList =>
                    {
                        var isSubscribe = request.Param.User.SubscribeWishLists.Any(wl => wl.Id == wishList.Id)
                            ? "✔️ " : "";
                        return new List<InlineKeyboardButton>
                        {
                            InlineKeyboardButton.WithCallbackData(
                            $"{isSubscribe}{wishList.Name}",
                            $"{Commands.UsersWishListSubscribeRequest}<?>{wishList.Id}"),
                        };
                    }).ToList();
            }

            keyboard.AddBaseFooter();

            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
        else
        {
            throw new DomainException(BaseMessages.CommandNotRecognized);
        }
    }
}