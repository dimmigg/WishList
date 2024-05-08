﻿using System.Text;
using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.WishLists;

namespace WishList.Domain.UseCases.MyWishLists.MyWishListInfo;

public class MyWishListInfoUseCase(
    ITelegramSender telegramSender,
    IWishListStorage wishListStorage)
    : IRequestHandler<MyWishListInfoCommand>
{
    public async Task Handle(MyWishListInfoCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var wishListId))
        {
            var wishList = await wishListStorage.GetWishList(wishListId, cancellationToken);
            if (wishList == null) return;
            var sb = new StringBuilder($"Список: *{wishList.Name.MarkForbiddenChar()}*\n");
            sb.AppendLine($"Кол\\-во записей: *{wishList.Presents.Count}*");
            var isPrivate = wishList.IsPrivate ? "вкл" : "выкл";
            sb.AppendLine($"Приватность: *{isPrivate}*");

            List<List<InlineKeyboardButton>> keyboard =
            [
                [
                    InlineKeyboardButton.WithCallbackData(
                        "✏️ Изменить список", $"{Commands.MY_PRESENTS}<?>{wishListId}"),
                    InlineKeyboardButton.WithCallbackData(
                        "⚙ Параметры списка", $"{Commands.MY_WISH_LIST_PARAMS}<?>{wishListId}")
                ],
                [
                    InlineKeyboardButton.WithCallbackData(
                        "🗑 Удалить список", $"{Commands.MY_WISH_LIST_DELETE_REQUEST}<?>{wishListId}")
                ],
            ];

            keyboard = keyboard.AddBaseFooter(Commands.MY_WISH_LISTS);

            await telegramSender.EditMessageAsync(
                text: sb.ToString(),
                replyMarkup: new InlineKeyboardMarkup(keyboard),
                cancellationToken: cancellationToken);
        }
    }
}