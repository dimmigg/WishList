using Telegram.Bot;
using WishList.Bot.Abstract;

namespace WishList.Bot.Services;

public class ReceiverService(
    ITelegramBotClient botClient,
    UpdateHandlers updateHandler)
    : ReceiverServiceBase<UpdateHandlers>(botClient, updateHandler);
