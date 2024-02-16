using WishList.Domain.Exceptions;
using WishList.Storage.CommandOptions;

namespace Telegram.Bot.Types;

public static class CallbackQueryExtensions
{
    public static void ParseCommand(this CallbackQuery callbackQuery,  out Command way, out CommandStep step)
    {
        if(callbackQuery.Data == null) throw new DomainException("Команда не распознана");
        callbackQuery.Data.ParseCommand(out way, out step);
    }
}