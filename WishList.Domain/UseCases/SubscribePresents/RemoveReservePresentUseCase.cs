using WishList.Domain.Models;
using WishList.Domain.TelegramSender;
using WishList.Storage.Storages.Presents;

namespace WishList.Domain.UseCases.SubscribePresents;

public class RemoveReservePresentUseCase(
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
            await presentStorage.RemoveReserve(presentId, cancellationToken);

            await sender.AnswerCallbackQueryAsync(
                param.CallbackQuery.Id,
                "Подарок удалён из зарезервирова!",
                cancellationToken: cancellationToken);
            var fromReserve = command.Length == 3 ? "<?>r" : "";
            param.Command = $"spi<?>{presentId}{fromReserve}";
            var useCase = new SubscribePresentInfoUseCase(param, sender, presentStorage);
            await useCase.Execute(cancellationToken);
        }
    }
}