using Telegram.Bot.Types;

namespace WishList.Domain.Models;

public class UseCaseParam
{
    public string Command { get; set; }
    public RegisteredUser User { get; set; }
    public Message? Message  { get; set; }
    public CallbackQuery? CallbackQuery  { get; set; }
    public string[] Commands => Command.Split("<?>");
    public string LastCommand => Commands[0];
}