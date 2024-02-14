using Telegram.Bot.Types;

namespace WishList.Domain.Received;

public interface IReceivedService
{
    Task MessageReceivedAsync(Message message, CancellationToken cancellationToken);
    Task CallbackQueryReceivedAsync(Telegram.Bot.Types.CallbackQuery callbackQuery, CancellationToken cancellationToken);
    Task InlineQueryReceivedAsync(InlineQuery inlineQuery, CancellationToken cancellationToken);
    Task ChosenInlineResultReceivedAsync(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken);
    Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken);
}