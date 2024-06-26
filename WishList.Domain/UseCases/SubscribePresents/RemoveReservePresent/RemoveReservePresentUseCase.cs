﻿using MediatR;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.SubscribePresents.SubscribePresentInfo;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.SubscribePresents.RemoveReservePresent;

public class RemoveReservePresentUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage,
    IMediator mediator)
    : IRequestHandler<RemoveReservePresentCommand>
{
    public async Task Handle(RemoveReservePresentCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.Commands.Length < 2) return;
        if (int.TryParse(request.Param.Commands[1], out var presentId))
        {
            await presentStorage.RemoveReserve(presentId, cancellationToken);

            await telegramSender.AnswerCallbackQueryAsync(
                "Подарок удалён из резервирова. Теперь его может зарезервировать кто-нибудь другой.",
                cancellationToken: cancellationToken);
            var fromReserve = request.Param.Commands.Length == 3 ? $"<?>{Commands.Reserved}" : "";
            
            request.Param.Command = $"{Commands.SubscribePresentInfo}<?>{presentId}{fromReserve}";
            await mediator.Send(new SubscribePresentInfoCommand(request.Param), cancellationToken);
        }
    }
}