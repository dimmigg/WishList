using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using WishList.Domain.Received;

namespace WishList.Bot.Services;

public class UpdateHandlers(IReceivedService received,
    ILogger<UpdateHandlers> logger)  : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            var handler = update switch
            {
                { Message: { } message } => received.MessageReceivedAsync(message, cancellationToken),
                { EditedMessage: { } message } => received.MessageReceivedAsync(message, cancellationToken),
                { CallbackQuery: { } callbackQuery } => received.CallbackQueryReceivedAsync(callbackQuery,
                    cancellationToken),
                _ => received.UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error with HandleUpdateAsync");
            Console.WriteLine(e);
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Error with HandleUpdateAsync");
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }
}