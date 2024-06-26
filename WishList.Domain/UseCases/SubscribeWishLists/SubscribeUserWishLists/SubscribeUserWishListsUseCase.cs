﻿using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.SubscribeWishLists.SubscribeWishListInfo;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.SubscribeWishLists.SubscribeUserWishLists;

public class SubscribeUserWishListsUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage,
    IMediator mediator)
    : IRequestHandler<SubscribeUserWishListsCommand>
{
    public async Task Handle(SubscribeUserWishListsCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Commands.Length < 2) return;

        if (long.TryParse(request.Param.Commands[1], out var subscribeUserId))
        {
            List<List<InlineKeyboardButton>> keyboard = [];
            var wishLists = (await wishListStorage.GetSubscribeWishLists(request.Param.User.Id, cancellationToken))
                .Where(wl => wl.AuthorId == subscribeUserId)
                .ToArray();

            if (wishLists.Length == 1)
            {
                request.Param.Command = $"{Commands.SubscribeWishListInfo}<?>{wishLists.First().Id}";
                await mediator.Send(new SubscribeWishListInfoCommand(request.Param), cancellationToken);
            }
            else
            {
                var sb = new StringBuilder();
                if (wishLists.Length != 0)
                {
                    var author = wishLists.First().Author;
                    sb.AppendLine($"Списки пользователя {author}:");
                    keyboard = wishLists
                        .Select(wishList => new List<InlineKeyboardButton>
                        {
                            InlineKeyboardButton.WithCallbackData($"{wishList.Name} ({wishList.Presents.Count})",
                                $"{Commands.SubscribeWishListInfo}<?>{wishList.Id}"),
                        }).ToList();
                }
                else
                {
                    sb.AppendLine("Еще нет подписок");
                }

                keyboard.AddBaseFooter(Commands.SubscribeUsers);

                await telegramSender.EditMessageAsync(
                    text: sb.ToString().MarkForbiddenChar(),
                    replyMarkup: new InlineKeyboardMarkup(keyboard),
                    cancellationToken: cancellationToken);
            }
        }
    }
}