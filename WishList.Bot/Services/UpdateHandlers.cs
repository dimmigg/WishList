using Telegram.Bot.Types;
using WishList.Domain.Received;
namespace WishList.Bot.Services;

public class UpdateHandlers(IReceivedService received)
{
    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message }                       => received.MessageReceivedAsync(message, cancellationToken),
            { EditedMessage: { } message }                 => received.MessageReceivedAsync(message, cancellationToken),
            { CallbackQuery: { } callbackQuery }           => received.CallbackQueryReceivedAsync(callbackQuery, cancellationToken),
            { InlineQuery: { } inlineQuery }               => received.InlineQueryReceivedAsync(inlineQuery, cancellationToken),
            { ChosenInlineResult: { } chosenInlineResult } => received.ChosenInlineResultReceivedAsync(chosenInlineResult, cancellationToken),
            _                                              => received.UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }
}
