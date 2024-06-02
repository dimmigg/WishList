using Telegram.Bot;
using Telegram.Bot.Polling;

namespace WishList.Bot.Abstract;

public abstract class ReceiverServiceBase<TUpdateHandler>(
    ITelegramBotClient botClient,
    TUpdateHandler updateHandler
    ) : IReceiverService
    where TUpdateHandler : IUpdateHandler
{
    public async Task ReceiveAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = [],
            ThrowPendingUpdates = true,
        };

        await botClient.ReceiveAsync(
            updateHandler: updateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken);
    }
}
