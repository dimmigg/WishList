﻿using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.SubscribePresents;

public class ReservePresentUseCase(
    UseCaseParam param,
    ISender sender,
    IPresentStorage presentStorage)
    : IUseCase
{

    public async Task Execute(CancellationToken cancellationToken)
    {
        if (param.CallbackQuery == null) return;

        var command = param.Command.Split("<?>");
        if (command.Length < 3) return;
        if (int.TryParse(command[1], out var presentId) &&
            long.TryParse(command[2], out var reservedUserId))
        {
            await presentStorage.Reserve(presentId, reservedUserId, cancellationToken);

            await sender.AnswerCallbackQueryAsync(
                param.CallbackQuery.Id,
                "Подарок зарезервирован!",
                cancellationToken: cancellationToken);

            param.Command = $"spi<?>{presentId}";
            var useCase = new SubscribePresentInfoUseCase(param, sender, presentStorage);
            await useCase.Execute(cancellationToken);
        }
    }
}