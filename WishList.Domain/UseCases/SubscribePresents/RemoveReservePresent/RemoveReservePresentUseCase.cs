using MediatR;
using WishList.Domain.UseCases.SubscribePresents.SubscribePresentInfo;
using WishList.Storage.Storages.Presents;
using ISender = WishList.Domain.TelegramSender.ISender;

namespace WishList.Domain.UseCases.SubscribePresents.RemoveReservePresent;

public class RemoveReservePresentUseCase(
    ISender sender,
    IPresentStorage presentStorage,
    IMediator mediator)
    : IRequestHandler<RemoveReservePresentCommand>
{
    public async Task Handle(RemoveReservePresentCommand request, CancellationToken cancellationToken)
    {
        if (request.Param.CallbackQuery == null) return;

        var command = request.Param.Command.Split("<?>");
        if (command.Length < 2) return;
        if (int.TryParse(command[1], out var presentId))
        {
            await presentStorage.RemoveReserve(presentId, cancellationToken);

            await sender.AnswerCallbackQueryAsync(
                request.Param.CallbackQuery.Id,
                "Подарок удалён из зарезервирова!",
                cancellationToken: cancellationToken);
            var fromReserve = command.Length == 3 ? $"<?>{Commands.RESERVED}" : "";
            
            request.Param.Command = $"{Commands.SUBSCRIBE_PRESENT_INFO}<?>{presentId}{fromReserve}";
            await mediator.Send(new SubscribePresentInfoCommand(request.Param), cancellationToken);
        }
    }
}