using MediatR;
using WishList.Domain.Constants;
using WishList.Domain.TelegramSender;
using WishList.Domain.UseCases.SubscribePresents.SubscribePresentInfo;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.SubscribePresents.ReservePresent;

public class ReservePresentUseCase(
    ITelegramSender telegramSender,
    IPresentStorage presentStorage,
    IMediator mediator)
    : IRequestHandler<ReservePresentCommand>
{
    public async Task Handle(ReservePresentCommand request, CancellationToken cancellationToken)
    {
        var command = request.Param.Command.Split("<?>");
        if (command.Length < 3) return;
        if (int.TryParse(command[1], out var presentId) &&
            long.TryParse(command[2], out var reservedUserId))
        {
            await presentStorage.Reserve(presentId, reservedUserId, cancellationToken);

            await telegramSender.AnswerCallbackQueryAsync(
                "Подарок зарезервирован!",
                cancellationToken: cancellationToken);

            request.Param.Command = $"{Commands.SUBSCRIBE_PRESENT_INFO}<?>{presentId}";
            await mediator.Send(new SubscribePresentInfoCommand(request.Param), cancellationToken);
        }
    }
}