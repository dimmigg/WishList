using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using WishList.Bot.Filters;
using WishList.Bot.Services;

namespace WishList.Bot.Controllers;

public class BotController : ControllerBase
{
    [HttpPost]
    [ValidateTelegramBot]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}
