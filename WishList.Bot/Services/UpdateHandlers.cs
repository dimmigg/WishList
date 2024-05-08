using Telegram.Bot.Types;
using WishList.Domain.Received;

namespace WishList.Bot.Services;

public class UpdateHandlers(IReceivedService received)
{
    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
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
            Console.WriteLine(e);
        }
    }
}