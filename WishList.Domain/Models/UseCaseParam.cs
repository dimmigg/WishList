using Telegram.Bot.Types;
using WishList.Storage.Entities;

namespace WishList.Domain.Models;

public class UseCaseParam
{
    public string Command { get; set; }
    public TelegramUser User { get; set; }
    public Message? Message  { get; set; }
    public CallbackQuery? CallbackQuery  { get; set; }
    public string[] Commands => Command.Split("<?>");
    public string LastCommand => Commands[0];
}